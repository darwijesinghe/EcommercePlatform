using EP.User.Data.Persistence;
using EP.User.Data.Repository.Classes;
using EP.User.Helpers.Extensions;
using EP.User.Services.Classes;
using EP.User.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Services.Interfaces;

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

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Application services
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IUserService                , UserService>();

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