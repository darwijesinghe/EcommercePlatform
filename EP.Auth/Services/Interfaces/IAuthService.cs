using SharedLibrary.DTOs;
using SharedLibrary.Response;

namespace EP.Auth.Services.Interfaces
{
    /// <summary>
    /// Defines contracts of AUTH related functionalities.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Stores the user with the refresh token.
        /// </summary>
        /// <param name="userId">The registered user's unique  identification.</param>
        /// <param name="token">The refresh token issued to the user.</param>
        /// <param name="expiry">The expire data of the refresh token.</param>
        /// <returns>
        /// The type of <see cref="Result"/> that contains the storing process result.
        /// </returns>
        Task<Result> StoreUserAsync(int userId, string token, DateTime expiry);

        /// <summary>
        /// Retrieves the refresh token from the database.
        /// </summary>
        /// <param name="token">The previously issued refresh token.</param>
        /// <returns>
        /// The type of <see cref="Result{UserAuthDto}"/> that contains the user AUTH data.
        /// </returns>
        Task<Result<UserAuthDto>> GetRefreshTokenAsync(string token);

        /// <summary>
        /// Revokes the refresh token for new one (rotation).
        /// </summary>
        /// <param name="token">The refresh token issued to the user.</param>
        /// <returns>
        /// The type of <see cref="Result"/> that contains the revoke process result.
        /// </returns>
        Task<Result> RevokeAsync(string token);
    }
}
