using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Settings;
using System.Text;

namespace EP.Auth.Helpers.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring and registering application services
    /// into the ASP.NET Core dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers authentication and authorization services into the dependency injection container.
        /// This may include JWT bearer authentication, identity services, and related configurations.
        /// </summary>
        /// <param name="services">The service collection to add authentication services to.</param>
        /// <returns>
        /// The updated <see cref="IServiceCollection"/> for chaining.
        /// </returns>
        public static IServiceCollection AddJwtAuthenticationService(this IServiceCollection services)
        {
            // JWT authentication setup
           services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // create a temporary service provider to resolve IOptions<DatabaseConfig>
                using var serviceProvider = services.BuildServiceProvider();
                var jwt                   = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

                // JWT secret
                var keyBytes = Encoding.UTF8.GetBytes(jwt.Secret);
                var key      = new SymmetricSecurityKey(keyBytes);

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,            // ensures the token's signature is valid (prevents tampering)
                    IssuerSigningKey         = key,             // secret key used to verify the token's signature
                    ValidateIssuer           = true,            // validates that the token was issued by a trusted issuer
                    ValidIssuer              = jwt.Issuer,      // the expected issuer value (must match the "iss" claim in the JWT token)
                    ValidateAudience         = true,            // validates that the token is intended for your application
                    ValidAudience            = jwt.Audience,    // the expected audience value (must match the "aud" claim in the JWT token)
                    RequireExpirationTime    = true,            // ensures the token has an expiration time
                    ValidateLifetime         = true,            // validates that the token is not expired
                    ClockSkew                = TimeSpan.Zero    // no additional time allowed after token expiration (default is 5 minutes, here it's set to 0)
                };

                // debugging
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"❌ Token validation failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
