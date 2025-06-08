using EP.Inventory.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedLibrary.DTOs;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;
using System.Text;

namespace EP.Inventory.Services.Classes
{
    /// <summary>
    /// Consumes product creation messages from the message broker.
    /// </summary>
    public class ProductCreatedConsumer : BackgroundService
    {
        // Services
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRabbitMqConnection  _connection;
        private readonly RabbitMqSettings     _settings;
        private readonly MessageQueues        _queues;

        public ProductCreatedConsumer(IServiceScopeFactory scopeFactory, IRabbitMqConnection connection, 
            IOptions<RabbitMqSettings> settings, IOptions<MessageQueues> queues)
        {
            _connection   = connection;
            _settings     = settings.Value;
            _queues       = queues.Value;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // creates a new channel
                using var channel = _connection.CreateModel();

                // declares the queue to ensure it exists before consuming messages
                channel.QueueDeclare(queue: _queues.ProductCreated, durable: false, exclusive: false, autoDelete: false);

                // creates a new consumer that will be used to receive messages from the queue
                // it is tied to the current channel
                var consumer = new EventingBasicConsumer(channel);

                // event handler that gets called when a new message is received from the queue
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();                                // retrieves the message body as a byte array
                    var json = Encoding.UTF8.GetString(body);                    // decodes the message body as a UTF-8 string
                    var data = JsonConvert.DeserializeObject<ProductDto>(json);  // converts to object

                    // inventory update
                    if(data is not null)
                    {
                        // gets the required service(s)
                        using var scope = _scopeFactory.CreateScope();
                        var repository  = scope.ServiceProvider.GetRequiredService<IGenericRepository<InventoryItem>>();

                        // checks the product existence
                        var product = await repository.IsExist(x => x.Id == data.Id);
                        if (!product)
                        {
                            // mapping the data
                            var obj = new InventoryItem
                            {
                                ProductId   = data.Id.Value,
                                ProductName = data.Name,
                                Quantity    = data.Quantity
                            };

                            await repository.AddAsync(obj);
                            await repository.SaveChangesAsync();

                            // just log to console
                            Console.WriteLine($"✅ Inventory item created: {obj.ProductName} x{obj.Quantity}");
                        }
                        else
                        {
                            // just log to console
                            Console.WriteLine($"✅ This product is already exist in the inventory: {data.Name}");
                        }
                    }
                };

                // starts consuming messages from the specified queue using the given consumer
                channel.BasicConsume(queue: _queues.ProductCreated, autoAck: true, consumer: consumer);

                // keeps the service alive until cancellation
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException ex)
            {
                // service was canceled — safe to exit
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
