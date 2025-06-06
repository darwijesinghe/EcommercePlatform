namespace SharedLibrary.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for publishing messages to a message queue or broker (e.g., RabbitMQ).
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// Publishes a message of type <typeparamref name="T"/> to the configured message queue.
        /// </summary>
        /// <param name="message">The message object to be published.</param>
        /// <returns>
        /// Returns <c>null</c> on success; otherwise error message.
        /// </returns>
        string Publish<T>(T message);
    }
}
