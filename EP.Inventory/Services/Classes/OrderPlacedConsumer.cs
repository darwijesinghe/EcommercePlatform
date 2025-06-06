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
    /// Consumes order placed messages from the message broker.
    /// </summary>
    public class OrderPlacedConsumer : BackgroundService
    {
        // Services
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRabbitMqConnection  _connection;
        private readonly RabbitMqSettings     _settings;
        private readonly MessageQueues        _queues;

        public OrderPlacedConsumer(IServiceScopeFactory scopeFactory, IRabbitMqConnection connection, 
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
                channel.QueueDeclare(queue: _queues.OrderPlaced, durable: false, exclusive: false, autoDelete: false);

                // creates a new consumer that will be used to receive messages from the queue
                // it is tied to the current channel
                var consumer = new EventingBasicConsumer(channel);

                // event handler that gets called when a new message is received from the queue
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();                              // retrieves the message body as a byte array
                    var json = Encoding.UTF8.GetString(body);                  // decodes the message body as a UTF-8 string
                    var data = JsonConvert.DeserializeObject<OrderDto>(json);  // converts to object

                    // inventory update
                    if (data is not null)
                    {
                        // just log to console
                        Console.WriteLine($"✅ A new order is placed by: {data.UserId} :{data.TotalAmount:C}");

                        // gets the required service(s)
                        using var scope = _scopeFactory.CreateScope();
                        var repository  = scope.ServiceProvider.GetRequiredService<IGenericRepository<InventoryItem>>();

                        // accessing the products in the order
                        foreach (var item in data.Items)
                        {
                            // checks the product existence
                            var product = await repository.GetByConditionAsync(x => x.ProductId == item.ProductId);
                            if (product is not null && product.Quantity >= 0)
                            {
                                // reduce the quantity
                                product.Quantity -= 1;

                                repository.Update(product);
                                await repository.SaveChangesAsync();
                            }
                        }
                    }
                };

                // starts consuming messages from the specified queue using the given consumer
                channel.BasicConsume(queue: _queues.OrderPlaced, autoAck: true, consumer: consumer);

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
