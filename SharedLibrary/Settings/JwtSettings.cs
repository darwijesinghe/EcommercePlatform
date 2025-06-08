namespace SharedLibrary.Settings
{
    /// <summary>
    /// Configuration settings for JWT, loaded from appsettings.json.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// The secret key used to sign the JWT.
        /// This should be a long, unique, and secure string that is used for token encryption and validation.
        /// </summary>
        public string Secret                { get; set; }

        /// <summary>
        /// The issuer of the JWT.
        /// This is typically the server or application that generates the token (e.g. "https://yourapp.com").
        /// </summary>
        public string Issuer                { get; set; }

        /// <summary>
        /// The audience for the JWT.
        /// This represents the intended recipient of the token, usually the application or service consuming the token.
        /// </summary>
        public string Audience              { get; set; }

        /// <summary>
        /// The duration in minutes for which the access token will remain valid.
        /// Once the token expires, it can no longer be used and must be refreshed or a new token generated.
        /// </summary>
        public int AccessTokenExpiryMinutes { get; set; }

        /// <summary>
        /// The duration in minutes for which the refresh token will remain valid.
        /// Once the token expires, it can no longer be used and must be refreshed or a new token generated.
        /// </summary>
        public int RefreshTokenExpiryDays   { get; set; }
    }
}
