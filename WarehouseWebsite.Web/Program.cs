using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Application.Services;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Web.Configure;
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
                .SetUpJsonOptions();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.SetUpSwagger();

            builder.Services.AddMemoryCache();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.SetUpIdentity(builder.Configuration);
            builder.Services.SetUpOptions();

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<JwtTokenService>();

            var app = builder.Build();

            app.Services.DatabaseMigrate();
            app.Services.SeedRoles();
            app.Services.SeedWithTestData();
            app.Services.CreateAzureBlobContainer();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
