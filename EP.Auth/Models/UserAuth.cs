namespace EP.Auth.Models
{
    /// <summary>
    /// Domain model for user auth data.
    /// </summary>
    public class UserAuth
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public int Id                { get; set; }

        /// <summary>
        /// The registered user's unique identifier.
        /// </summary>
        public int UserId            { get; set; }

        /// <summary>
        /// The refresh token issued to the user.
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// The refresh token expiration date.
        /// </summary>
        public DateTime ExpiryDate   { get; set; }

        /// <summary>
        /// Indicates whether the refresh token has been revoked before its expiry.
        /// </summary>
        /// <remarks>
        /// Common cases for revocation include:
        /// <list type="number">
        /// <item><description>User logs out</description></item>
        /// <item><description>User logs in from a new device</description></item>
        /// <item><description>Suspicious activity detected</description></item>
        /// <item><description>Token rotation (optional)</description></item>
        /// </list>
        /// </remarks>
        public bool IsRevoked        { get; set; }
    }
}
