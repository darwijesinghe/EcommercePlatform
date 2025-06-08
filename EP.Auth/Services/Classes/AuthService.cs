using EP.Auth.Models;
using EP.Auth.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using SharedLibrary.Response;
using SharedLibrary.Services.Interfaces;

namespace EP.Auth.Services.Classes
{
    /// <summary>
    /// Provides an implementation of <see cref="IAuthService"/> that manages the AUTH functionalities.
    /// </summary>
    public class AuthService : IAuthService
    {
        // Repositories
        private readonly IGenericRepository<UserAuth> _repository;

        public AuthService(IGenericRepository<UserAuth> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<Result> StoreUserAsync(int userId, string token, DateTime expiry)
        {
            try
            {
                // validations
                if (userId <= 0 || string.IsNullOrEmpty(token) || expiry == default)
                    return new Result { Message = "Required data not found." };

                // data object
                var auth = new UserAuth
                {
                    UserId     = userId,
                    Token      = token,
                    ExpiryDate = expiry,
                    IsRevoked  = false
                };

                // operation
                await _repository.AddAsync(auth);
                await _repository.SaveChangesAsync();

                return new Result { Success = true };
            }
            catch (Exception ex)
            {
                return new Result<UserDto> { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<UserAuthDto>> GetRefreshTokenAsync(string token)
        {
            try
            {
                // validations
                if(string.IsNullOrEmpty(token))
                    return new Result<UserAuthDto> { Message = "Refresh token is not valid." };

                // gets the token
                var entity = await _repository.GetByConditionAsync(x => x.Token == token && !x.IsRevoked);
                if (entity is null)
                    return new Result<UserAuthDto> { Message = "Refresh token is not valid." };

                // data mapping
                var data = entity.Adapt<UserAuthDto>();

                return new Result<UserAuthDto> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<UserAuthDto> { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result> RevokeAsync(string token)
        {
            try
            {
                // validations
                if (string.IsNullOrEmpty(token))
                    return new Result { Message = "Refresh token is not valid." };

                // gets the data using the token
                var entity = await _repository.GetByConditionAsync(x => x.Token == token);
                if (entity is null)
                    return new Result { Message = "Required data not found." };

                // marks as revoked
                entity.IsRevoked = true;

                // operation
                _repository.Update(entity);
                await _repository.SaveChangesAsync();

                return new Result { Success = true };
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message };
            }
        }
    }
}
