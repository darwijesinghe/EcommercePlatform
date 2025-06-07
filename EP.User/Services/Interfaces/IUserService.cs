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
        /// <param name="data">The type of <see cref="UserDto"/> that contains the registration data.</param>
        /// <returns>
        /// The type of <see cref="AuthResponse"/> that contains the registration process result.
        /// </returns>
        Task<AuthResponse> RegisterUserAsync(UserDto data);

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{IEnumerable{UserDto}}"/> containing a list of system users.
        /// </returns>
        Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
    }
}
