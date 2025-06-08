using EP.Product.Data.Persistence;
using EP.Product.Data.Repository.Classes;
using EP.Product.Helpers.Extensions;
using EP.Product.Services.Classes;
using EP.Product.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Shared.Classes;
using SharedLibrary.Services.Classes;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();

        // Application settings
        builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        // Database service
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        // Add swagger
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ep.Product", Version = "v1" });

            // Define the Bearer token authentication scheme
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name         = "Authorization",
                Type         = SecuritySchemeType.Http,
                Scheme       = "bearer",
                BearerFormat = "JWT",
                In           = ParameterLocation.Header,
                Description  = "Enter your valid JWT token."
            });

            // Add a global security requirement for the Bearer token and header
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type  = ReferenceType.SecurityScheme,
                            Id    = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        // JWT authentication setup
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // accessing settings
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            var jwtSettings        = jwtSettingsSection.Get<JwtSettings>();

            // JWT secret
            var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Secret);
            var key      = new SymmetricSecurityKey(keyBytes);

            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,                    // ensures the token's signature is valid (prevents tampering)
                IssuerSigningKey         = key,                     // secret key used to verify the token's signature
                ValidateIssuer           = true,                    // validates that the token was issued by a trusted issuer
                ValidIssuer              = jwtSettings.Issuer,      // the expected issuer value (must match the "iss" claim in the JWT token)
                ValidateAudience         = true,                    // validates that the token is intended for your application
                ValidAudience            = jwtSettings.Audience,    // the expected audience value (must match the "aud" claim in the JWT token)
                RequireExpirationTime    = true,                    // ensures the token has an expiration time
                ValidateLifetime         = true,                    // validates that the token is not expired
                ClockSkew                = TimeSpan.Zero            // no additional time allowed after token expiration (default is 5 minutes, here it's set to 0)
            };

#if DEBUG
            // for debugging
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Token validation failed: {context.Exception.Message}.");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine($"OnChallenge error: {context.Error}.");
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    var token  = context.HttpContext.Request.Headers["Authorization"];

                    if (string.IsNullOrEmpty(token))
                    {
                        Console.WriteLine("Token is null or empty.");
                        return Task.CompletedTask;
                    }

                    Console.WriteLine($"Token received: {token}.");
                    return Task.CompletedTask;
                }
            };
#endif

        });

        builder.Services.AddAuthorization();

        // Application services
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddSingleton<IRabbitMqConnection      , RabbitMqConnection>();
        builder.Services.AddSingleton<IMessagePublisher        , MessagePublisher>();
        builder.Services.AddScoped<IProductService             , ProductService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // for debugging
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Authorization: {context.Request.Headers["Authorization"]}.");
                await next();
            });
        }

        app.UseHttpsRedirection();

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            // Apply pending migrations
            app.ApplyPendingMigrations();
        }

        app.Run();
    }
}
