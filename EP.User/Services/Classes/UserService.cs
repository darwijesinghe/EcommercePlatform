using EP.User.Services.Interfaces;
using Mapster;
using SharedLibrary.DTOs;
using SharedLibrary.Enums;
using SharedLibrary.Helpers.Extensions;
using SharedLibrary.Response;
using SharedLibrary.Services.Interfaces;

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
        public async Task<Result> RegisterUserAsync(RegisterDto data)
        {
            try
            {
                // validations
                if (data is null)
                    return new Result { Message = "Required data not found." };

                // checks the user existence
                var emails = await _repository.GetAllAsync();
                if (emails is not null && emails.Any(x => x.Email.Equals(data.Email)))
                    return new Result { Message = "Email address already exist in the system." };

                // role validation
                bool exists = Enum.IsDefined(typeof(UserRoles), data.Role);
                if (!exists)
                    return new Result { Message = "This role is not valid user role." };

                // data mapping
                var user = data.Adapt<Models.User>();

                // operation
                await _repository.AddAsync(user);
                await _repository.SaveChangesAsync();

                // mapping to DTO
                var response = user.Adapt<UserDto>();

                return new Result { Success = true };
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                // gets all users
                var entities = await _repository.GetAllAsync();
                if (entities.IsNullOrEmpty())
                    return new Result<IEnumerable<UserDto>> { Message = "No data found." };

                // data mapping
                var data = entities.Adapt<IEnumerable<UserDto>>();

                return new Result<IEnumerable<UserDto>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<UserDto>> { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<UserDto>> GetUserAsync(int id)
        {
            try
            {
                // gets user by id
                var entity = await _repository.GetByConditionAsync(x => x.Id == id);
                if (entity is null)
                    return new Result<UserDto> { Message = "No data found." };

                // data mapping
                var data = entity.Adapt<UserDto>();

                return new Result<UserDto> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<UserDto> { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<UserDto>> ValidateUserAsync(LoginDto data)
        {
            try
            {
                // validations
                if (data is null)
                    return new Result<UserDto> { Message = "Required data not found." };

                // gets the user by email and password
                var entity = await _repository.GetByConditionAsync(x => x.Email.Equals(data.Email) && x.Password.Equals(data.Password));
                if (entity is null)
                    return new Result<UserDto> { Message = "Invalid credentials." };

                // data mapping
                var response = entity.Adapt<UserDto>();

                return new Result<UserDto> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new Result<UserDto> { Message = ex.Message };
            }
        }
    }
}
