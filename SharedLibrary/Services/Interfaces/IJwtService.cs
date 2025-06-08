using SharedLibrary.General;

namespace SharedLibrary.Services.Interfaces
{
    /// <summary>
    /// Defines the contracts for JWT-related functionalities.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token based on the user's name, email, and role.
        /// </summary>
        /// <param name="mail">The user's email address.</param>
        /// <param name="role">The user's role within the system.</param>
        /// <returns>
        /// Returns a JWT token as a string.
        /// </returns>
        string GenerateJwtToken(string mail, string role);

        /// <summary>
        /// Generates a random string of the specified length.
        /// </summary>
        /// <param name="length">The desired length of the random string.</param>
        /// <returns>
        /// Returns a randomly generated string.
        /// </returns>
        string GenerateRandomString(int length);

        /// <summary>
        /// Creates both a JWT token and a refresh token based on the provided data.
        /// </summary>
        /// <param name="data">The data required to generate the tokens.</param>
        /// <returns>
        /// Returns a tuple containing the JWT token, refresh token, and the expiration time.
        /// </returns>
        (string token, string refreshToken, int expiresAt) MakeTokens(MakeToken data);

    }
}
