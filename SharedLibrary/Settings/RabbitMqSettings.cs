namespace SharedLibrary.Settings
{
    /// <summary>
    /// Configuration settings for RabbitMQ, loaded from appsettings.json.
    /// </summary>
    public class RabbitMqSettings
    {
        /// <summary>
        /// The hostname or IP address of the RabbitMQ server.
        /// Example: "localhost" or "rabbitmq.myserver.com"
        /// </summary>
        public string HostName     { get; set; }

        /// <summary>
        /// The name of the queue to which the application will connect.
        /// Example: "task_queue".
        /// </summary>
        public string QueueName    { get; set; }

        /// <summary>
        /// The name of the exchange used for message routing.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// The type of the exchange.
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// The username used to authenticate with the RabbitMQ server.
        /// Example: "guest".
        /// </summary>
        public string UserName     { get; set; }

        /// <summary>
        /// The password used to authenticate with the RabbitMQ server.
        /// Example: "guest".
        /// </summary>
        public string Password     { get; set; }
    }
}
