namespace SharedLibrary.DTOs
{
    /// <summary>
    /// DTO model for refresh token.
    /// </summary>
    public class RefreshDto
    {
        /// <summary>
        /// The issued refresh token to the user.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
