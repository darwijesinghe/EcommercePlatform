using EP.Auth.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLibrary.DTOs;
using SharedLibrary.Response;
using SharedLibrary.Settings;
using System.Net;

namespace EP.Auth.Services.Classes
{
    /// <summary>
    /// Provides an implementation of <see cref="IUserServiceClient"/> that manages the user service functionalities.
    /// </summary>
    public class UserServiceClient : IUserServiceClient
    {
        // Services
        private readonly HttpClient  _httpClient;
        private readonly ApiSettings _settings;

        public UserServiceClient(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> settings)
        {
            _httpClient = httpClientFactory.CreateClient("GlobalClient");
            _settings   = settings.Value;
        }

        /// <inheritdoc/>
        public async Task<Result> RegisterUserAsync(RegisterDto data)
        {
            try
            {
                // validations
                if(data is null)
                    return new Result { Message = "Required data not found." };

                // sends the request
                var response = await _httpClient.PostAsJsonAsync("/api/user/register-user", data);

                // checks the response status
                if (response.StatusCode != HttpStatusCode.OK)
                    return new Result { Message = response.ReasonPhrase };

                // reads the response
                string result = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(result))
                {
                    // de-serializing the result
                    var user = JsonConvert.DeserializeObject<Result>(result);

                    return new Result { Success = user.Success, Message = user.Message };
                }

                return new Result<UserDto> { Message = "Error occurred." };
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

                // sends the request
                var response = await _httpClient.PostAsJsonAsync("/api/user/validate-user", data);

                // checks the response status
                if (response.StatusCode != HttpStatusCode.OK)
                    return new Result<UserDto> { Message = response.ReasonPhrase };

                // reads the response
                string result = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(result))
                {
                    // de-serializing the result
                    var user = JsonConvert.DeserializeObject<Result<UserDto>>(result);

                    return new Result<UserDto> { Success = user.Success, Message = user.Message, Data = user.Data };
                }

                return new Result<UserDto> { Message = "Error occurred." };
            }
            catch (Exception ex)
            {
                return new Result<UserDto> { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<UserDto>> GetUserAsync(int id)
        {
            try
            {
                // validations
                if (id <= 0)
                    return new Result<UserDto> { Message = "The user ID is not valid." };

                // query parameters
                var queryParams = new Dictionary<string, string?>
                {
                    ["id"] = id.ToString()
                };

                // build the query string
                var url      = QueryHelpers.AddQueryString("/api/user/get-user", queryParams);

                // sends the request
                var response = await _httpClient.GetAsync(url);

                // checks the response status
                if (response.StatusCode != HttpStatusCode.OK)
                    return new Result<UserDto> { Message = response.ReasonPhrase };

                // reads the response
                string result = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(result))
                {
                    // de-serializing the result
                    var user = JsonConvert.DeserializeObject<Result<UserDto>>(result);

                    return new Result<UserDto> { Success = user.Success, Message = user.Message, Data = user.Data };
                }

                return new Result<UserDto> { Message = "Error occurred." };
            }
            catch (Exception ex)
            {
                return new Result<UserDto> { Message = ex.Message };
            }
        }
    }
}
