using System.Text.Json.Serialization;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Application.Services;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Web.Identity;
using static WarehouseWebsite.Infrastructure.Models.InfrastructureExtensions;
using static WarehouseWebsite.Web.Identity.IdentityServices;

namespace WarehouseWebsite.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.SetUpIdentity(builder.Configuration);

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<JwtTokenService>();

            builder.Services.AddOptions<JwtSettings>()
                .BindConfiguration("Jwt")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var app = builder.Build();

            app.Services.DatabaseMigrate();
            app.Services.SeedRoles();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
