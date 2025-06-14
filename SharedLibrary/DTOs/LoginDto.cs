﻿using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs
{
    /// <summary>
    /// DTO model for user login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [EmailAddress]
        public required string Email    { get; set; }

        /// <summary>
        /// The user's password (should be hashed in real systems).
        /// </summary>
        public required string Password { get; set; }
    }
}
