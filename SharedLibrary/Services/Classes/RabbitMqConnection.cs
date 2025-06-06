using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;

namespace SharedLibrary.Services.Classes
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        // Connection instance
        private readonly IConnection _connection;

        public RabbitMqConnection(IOptions<RabbitMqSettings> options)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.Value.HostName,
                UserName = options.Value.UserName,
                Password = options.Value.Password
            };

            // fallback
            int retries = 5;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection attempt {i + 1} failed: {ex.Message}");

                    // wait 5 seconds
                    Thread.Sleep(5000); 
                }
            }

            // throw an error if all retries failed
            if (_connection is null)
                throw new InvalidOperationException("Failed to establish a RabbitMQ connection after multiple attempts.");
        }

        /// <inheritdoc/>
        public IModel CreateModel()
        {
            if (_connection is null || !_connection.IsOpen)
                throw new InvalidOperationException("Cannot create model: RabbitMQ connection is not open.");

            return _connection.CreateModel();
        }
    }
}
