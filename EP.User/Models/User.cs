using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EP.User.Models
{
    /// <summary>
    /// Domain model for user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public int Id                   { get; set; }

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
