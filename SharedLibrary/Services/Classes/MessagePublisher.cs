using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;
using System.Text;

namespace RabbitMQ.Shared.Classes
{
    /// <summary>
    /// Publishes messages to RabbitMQ.
    /// </summary>
    public class MessagePublisher : IMessagePublisher
    {
        // Services
        private readonly IRabbitMqConnection _connection;
        private readonly RabbitMqSettings    _settings;

        public MessagePublisher(IRabbitMqConnection connection, IOptions<RabbitMqSettings> settings)
        {
            _connection = connection;
            _settings   = settings.Value;
        }

        /// <inheritdoc/> 
        public string Publish<T>(T message)
        {
            try
            {
                // creates a new channel
                using var channel = _connection.CreateModel();

                // declares the queue to ensure it exists before consuming messages.
                channel.QueueDeclare(queue: _settings.QueueName, durable: false, exclusive: false, autoDelete: false);

                // prepares the message
                string jsonString = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                // converts message to bytes
                var body = Encoding.UTF8.GetBytes(jsonString);

                // publishing the message
                channel.BasicPublish(exchange: "", routingKey: _settings.QueueName, basicProperties: null, body: body);

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
