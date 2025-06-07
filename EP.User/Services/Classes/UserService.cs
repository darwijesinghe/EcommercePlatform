using EP.User.Services.Interfaces;
using Mapster;
using SharedLibrary.DTOs;
using SharedLibrary.Helpers.Extensions;
using SharedLibrary.Response;
using SharedLibrary.Services.Interfaces;
using System.Numerics;

namespace EP.User.Services.Classes
{
    /// <summary>
    /// Provides an implementation of <see cref="IUserService"/> that manages the user data functionalities.
    /// </summary>
    public class UserService : IUserService
    {
        // Repositories
        private readonly IGenericRepository<Models.User> _repository;

        public UserService(IGenericRepository<Models.User> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<AuthResponse> RegisterUserAsync(UserDto data)
        {
            try
            {
                // validations
                if (data is null)
                    return new AuthResponse { Message = "Required data not found." };

                // checks the user existence
                var emails = await _repository.GetAllAsync();
                if (emails is not null && emails.Any(x => x.Email.Equals(data.Email)))
                    return new AuthResponse { Message = "Email address already exist in the system." };

                // data mapping
                var user = data.Adapt<Models.User>();

                // operation
                await _repository.AddAsync(user);
                await _repository.SaveChangesAsync();

                return new AuthResponse { Success = true };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                // gets all users
                var users = await _repository.GetAllAsync();
                if (users.IsNullOrEmpty())
                    return new Result<IEnumerable<UserDto>> { Message = "No data found." };

                // data mapping
                var data = users.Adapt<IEnumerable<UserDto>>();

                return new Result<IEnumerable<UserDto>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<UserDto>> { Message = ex.Message };
            }
        }
    }
}
