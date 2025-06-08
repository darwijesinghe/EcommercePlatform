using SharedLibrary.DTOs;
using SharedLibrary.Response;

namespace EP.User.Services.Interfaces
{
    /// <summary>
    /// Defines contracts of user related functionalities.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers the new user to the system.
        /// </summary>
        /// <param name="data">The type of <see cref="RegisterDto"/> that contains the registration data.</param>
        /// <returns>
        /// A <see cref="Result"/> that contains the registration process result.
        /// </returns>
        Task<Result> RegisterUserAsync(RegisterDto data);

        /// <summary>
        /// Validates the user credentials.
        /// </summary>
        /// <param name="data">The type of <see cref="LoginDto"/> that contains the login data.</param>
        /// <returns>
        /// A <see cref="Result{UserDto}"/> that contains the validation process result.
        /// </returns>
        Task<Result<UserDto>> ValidateUserAsync(LoginDto data);

        /// <summary>
        /// Retrieves all the users in the system.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{IEnumerable{UserDto}}"/> containing a list of system users.
        /// </returns>
        Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();

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
