namespace SharedLibrary.Response
{
    /// <summary>
    /// Represents the result of an authentication operation, containing user details, 
    /// authentication tokens, and status information.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Authenticated user's username.
        /// </summary>
        public string UserName     { get; set; }

        /// <summary>
        /// The role assigned to the authenticated user.
        /// </summary>
        public string Role         { get; set; }

        /// <summary>
        /// A value indicating whether the authentication operation succeeded.
        /// A value of <c>true</c> means the login or token generation was successful.
        /// </summary>
        public bool Success        { get; set; } = false;

        /// <summary>
        /// A message describing the result of the authentication operation.
        /// Useful for conveying error reasons or confirmation messages.
        /// </summary>
        public string Message      { get; set; } = "OK.";

        /// <summary>
        /// The access token (JWT) issued upon successful authentication.
        /// This token is typically used to authorize subsequent API requests.
        /// </summary>
        public string Token        { get; set; }

        /// <summary>
        /// The refresh token used to obtain a new access token
        /// after the current one expires without re-authenticating the user.
        /// </summary>
        public string RefreshToken { get; set; }
    }

}
