using SharedLibrary.DTOs;
using SharedLibrary.Response;

namespace EP.Auth.Services.Interfaces
{
    /// <summary>
    /// Defines contracts of user service related functionalities.
    /// </summary>
    public interface IUserServiceClient
    {
        /// <summary>
        /// Registers the new user to the system.
        /// </summary>
        /// <param name="data">The type of <see cref="RegisterDto"/> that contains the registration data.</param>
        /// <returns>
        /// The type of <see cref="Result"/> that contains the registration process result.
        /// </returns>
        Task<Result> RegisterUserAsync(RegisterDto data);

        /// <summary>
        /// Validates the user credentials.
        /// </summary>
        /// <param name="data">The type of <see cref="LoginDto"/> that contains the login data.</param>
        /// <returns>
        /// The type of <see cref="Result{UserDto}"/> that contains the validation process result.
        /// </returns>
        Task<Result<UserDto>> ValidateUserAsync(LoginDto data);

        /// <summary>
        /// Retrieves the user from unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <returns>
        /// A <see cref="Result{UserDto}"/> containing a data of the user.
        /// </returns>
        Task<Result<UserDto>> GetUserAsync(int id);
    }
}
