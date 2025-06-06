using EP.Product.Data.Persistence;
using EP.Product.Data.Repository.Classes;
using EP.Product.Helpers.Extensions;
using EP.Product.Services.Classes;
using EP.Product.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Shared.Classes;
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

        // Repository service
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // RabbitMQ connection
        builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

        // RabbitMQ publisher
        builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

        // Business services
        builder.Services.AddScoped<IProductService, ProductService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
