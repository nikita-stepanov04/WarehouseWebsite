using System.Text.Json.Serialization;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Application.Services;
using static WarehouseWebsite.Infrastructure.Models.InfrastructureExtensions;

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

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            var app = builder.Build();

            DatabaseMigrate(app.Services);
            
            app.UseSwagger();
            app.UseSwaggerUI();            

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
