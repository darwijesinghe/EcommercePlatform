using EP.User.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;

namespace EP.User.Controllers
{
    /// <summary>
    /// Handles user-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Services
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user to the system.
        /// </summary>
        /// <param name="data">The type of <see cref="UserDto"/> that contains the registration data.</param>
        [HttpPost("register-user")]
        public async Task<JsonResult> RegisterNewUser([FromBody] UserDto data)
        {
            try
            {
                // validations
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Required data is not found." });

                // gets the result
                var result = await _userService.RegisterUserAsync(data);

                return new JsonResult(new { result.Success, result.Message });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all the registered users in the system.
        /// </summary>
        [HttpPost("get-all-user")]
        public async Task<JsonResult> GetAllUsers()
        {
            try
            {
                // gets the result
                var result = await _userService.GetAllUsersAsync();

                return new JsonResult(new { result.Success, result.Message, result.Data });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }
    }
}
