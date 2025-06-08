using EP.Auth.Data.Persistence;
using EP.Auth.Data.Repository.Classes;
using EP.Auth.Helpers.Extensions;
using EP.Auth.Services.Classes;
using EP.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Settings;
using System.Net.Http.Headers;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();

        // Application settings
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

        // accessing the API settings
        var apiSettingsSection = builder.Configuration.GetSection("ApiSettings");
        var apiSettings        = apiSettingsSection.Get<ApiSettings>();

        // Register a single HttpClient instance
        builder.Services.AddHttpClient(apiSettings.ClientName, client =>
        {
            client.BaseAddress = new Uri(apiSettings.BaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        // Database service
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // JWT authentication
        builder.Services.AddJwtAuthenticationService();

        // Application services
        builder.Services.AddSingleton<IJwtService              , JwtService>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IAuthService                , AuthService>();
        builder.Services.AddScoped<IUserServiceClient          , UserServiceClient>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Authentication & Authorization
        app.UseAuthentication();
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