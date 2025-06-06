using EP.Inventory.Data.Persistence;
using EP.Inventory.Data.Repository.Classes;
using EP.Inventory.Helpers.Extensions;
using EP.Inventory.Services.Classes;
using EP.Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Services.Classes;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Database service
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

        // RabbitMQ settings
        builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

        // RabbitMQ queues
        builder.Services.Configure<MessageQueues>(builder.Configuration.GetSection("MessageQueues"));

        // Repository service
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // RabbitMQ connection
        builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

        // Business services
        builder.Services.AddScoped<IInventoryService, InventoryService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Product created consumer
        builder.Services.AddHostedService<ProductCreatedConsumer>();

        // Order placed consumer
        builder.Services.AddHostedService<OrderPlacedConsumer>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            // Apply pending migrations
            app.ApplyPendingMigrations();
        }

        app.Run();
    }
}
