using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.General;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EP.Auth.Services.Classes
{
    /// <summary>
    /// Provides an implementation of <see cref="IJwtService"/> that handles the JWT functionalities.
    /// </summary>
    public class JwtService : IJwtService
    {
        // Services
        private readonly JwtSettings          _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtService(IHttpContextAccessor httpContext, IOptions<JwtSettings> jwtConfig)
        {
            _httpContextAccessor = httpContext;
            _jwtSettings         = jwtConfig.Value;
        }

        /// <inheritdoc/>
        public string GenerateJwtToken(string mail, string role)
        {
            // claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub  , mail),
                new Claim(JwtRegisteredClaimNames.Email, mail),
                new Claim(JwtRegisteredClaimNames.Jti  , Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat  , new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role              , role)
            };

            // Jwt secret
            var keyBytes      = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var key           = new SymmetricSecurityKey(keyBytes);

            // signing credentials
            var siCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // expire time
            var minutes       = _jwtSettings.AccessTokenExpiryMinutes;

            // setup token
            var token = new JwtSecurityToken
            (
                audience          : _jwtSettings.Audience,
                issuer            : _jwtSettings.Issuer,
                claims            : claims,
                notBefore         : DateTime.UtcNow,
                expires           : DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: siCredentials
            );

            // writes token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        /// <inheritdoc/>
        public string GenerateRandomString(int length)
        {
            // random obj
            var random         = new Random();

            // random chars
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

            // generates the random string
            return new string(Enumerable
                .Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)])
                .ToArray());
        }

        /// <inheritdoc/>
        public (string token, string refreshToken, int expiresAt) MakeTokens(MakeToken data)
        {
            // main token
            var token        = GenerateJwtToken(data.Mail, data.Role);

            // refresh token
            var refreshToken = GenerateRandomString(data.Length);

            // expiration
            var expiresAt    = _jwtSettings.AccessTokenExpiryMinutes;

            return (token, refreshToken, expiresAt);
        }
    }
}
