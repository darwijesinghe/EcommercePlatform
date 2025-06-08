using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs
{
    /// <summary>
    /// DTO model for user.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The user's unique identifier.
        /// </summary>
        public int? Id                  { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [EmailAddress]
        public required string Email    { get; set; }

        /// <summary>
        /// The user's password (should be hashed in real systems).
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// The user's role.
        /// </summary>
        public required string Role     { get; set; }
    }
}
