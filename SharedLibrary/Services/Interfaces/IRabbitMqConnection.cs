using RabbitMQ.Client;

namespace SharedLibrary.Services.Interfaces
{
    /// <summary>
    /// Interface for managing RabbitMQ connections.
    /// </summary>
    public interface IRabbitMqConnection
    {
        /// <summary>
        /// Creates and returns a new channel (IModel) for communicating with RabbitMQ.
        /// Channels are the primary means of sending and receiving messages.
        /// </summary>
        /// <returns>
        /// An open IModel (channel) connected to the RabbitMQ broker.
        /// </returns>
        IModel CreateModel();
    }
}
