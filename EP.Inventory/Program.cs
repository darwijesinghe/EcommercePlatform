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

        // Application settings
        builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
        builder.Services.Configure<MessageQueues>(builder.Configuration.GetSection("MessageQueues"));

        // Database service
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Application services
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddSingleton<IRabbitMqConnection      , RabbitMqConnection>();
        builder.Services.AddScoped<IInventoryService           , InventoryService>();
        builder.Services.AddHostedService<ProductCreatedConsumer>();
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
