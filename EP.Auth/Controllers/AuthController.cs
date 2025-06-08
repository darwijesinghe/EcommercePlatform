using Azure.Core;
using EP.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedLibrary.DTOs;
using SharedLibrary.General;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;

namespace EP.Auth.Controllers
{
    /// <summary>
    /// Handles AUTH related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Services
        private readonly IUserServiceClient _userServiceClient;
        private readonly IJwtService        _jwtService;
        private readonly IAuthService       _authService;
        private readonly JwtSettings        _jwtSettings;

        public AuthController(IUserServiceClient userServiceClient, IAuthService authService, IJwtService jwtService,
            IOptions<JwtSettings> settings)
        {
            _userServiceClient = userServiceClient;
            _authService       = authService;
            _jwtService        = jwtService;
            _jwtSettings       = settings.Value;
        }

        /// <summary>
        /// Registers a new user to the system.
        /// </summary>
        /// <param name="data">The type of <see cref="RegisterDto"/> that contains the registration data.</param>
        [HttpPost("register")]
        public async Task<JsonResult> Register([FromBody] RegisterDto data)
        {
            try
            {
                // validations
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Required data is not found." });

                // gets the result
                var result = await _userServiceClient.RegisterUserAsync(data);

                return new JsonResult(new { result.Success, result.Message });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }

        /// <summary>
        /// Login user to the system.
        /// </summary>
        /// <param name="data">The type of <see cref="LoginDto"/> that contains the login data.</param>
        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] LoginDto data)
        {
            try
            {
                // validations
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Required data is not found." });

                // validates the user
                var result = await _userServiceClient.ValidateUserAsync(data);
                if (!result.Success)
                    return new JsonResult(new { result.Success, result.Message });

                // gets the tokens
                var (token, refreshToken, expiresAt) = _jwtService.MakeTokens(new MakeToken
                {
                    Mail   = result.Data.Email,
                    Role   = result.Data.Role,
                    Length = 20
                });

                // saves the refresh token with the user
                await _authService.StoreUserAsync(result.Data.Id.Value, refreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays));

                // returns the response with the token data
                return new JsonResult(new
                {
                    success   = true,
                    message   = "OK.",
                    token,
                    refreshToken,
                    expiresAt
                });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }

        /// <summary>
        /// Sends new tokens to the user.
        /// </summary>
        /// <param name="data">The type of <see cref="RefreshDto"/> that contains the refresh token data.</param>
        [HttpPost("refresh")]
        public async Task<JsonResult> Refresh([FromBody] RefreshDto data)
        {
            try
            {
                // validations
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Required data is not found." });

                // gets the refresh token
                var result = await _authService.GetRefreshTokenAsync(data.RefreshToken);
                if (!result.Success || result.Data.ExpiryDate < DateTime.UtcNow)
                    return new JsonResult(new { success = false, message = "Invalid or expired refresh token." });

                // rotate refresh tokens
                await _authService.RevokeAsync(data.RefreshToken);

                // gets the user data to make the tokens
                var user = await _userServiceClient.GetUserAsync(result.Data.Id.Value);

                // gets the tokens
                var (token, refreshToken, expiresAt) = _jwtService.MakeTokens(new MakeToken
                {
                    Mail   = user.Data.Email,
                    Role   = user.Data.Role,
                    Length = 25
                });

                // saves the refresh token with the user
                await _authService.StoreUserAsync(result.Data.Id.Value, refreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays));

                // returns the response with the token data
                return new JsonResult(new
                {
                    success = true,
                    message = "OK.",
                    token,
                    refreshToken,
                    expiresAt
                });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }
    }
}
