namespace SharedLibrary.Settings
{
    /// <summary>
    /// Represents the message queue names.
    /// </summary>
    public class MessageQueues
    {
        // Order service queue names -------------
        public string OrderPlaced    { get; set; }

        // Product service queue names -----------
        public string ProductCreated { get; set; }
    }
}
