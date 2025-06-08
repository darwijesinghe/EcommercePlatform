namespace SharedLibrary.Settings
{
    /// <summary>
    /// Configuration settings for HttpClient, loaded from appsettings.json.
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// The name of the HttpClient.
        /// </summary>
        public string  ClientName { get; set; }

        /// <summary>
        /// Base address.
        /// </summary>
        public string BaseAddress { get; set; }
    }
}
