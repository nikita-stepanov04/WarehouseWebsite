using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using WarehouseWebsite.Infrastructure.Jobs;

namespace WarehouseWebsite.Infrastructure.Models
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, IConfiguration config)
        {
            string? dbConnection = config
                .GetConnectionString("DbConnection")
                .InjectEnvironmentVariables();

            if (dbConnection == null) throw new ArgumentNullException("DbConnection is not defined");
            
            services.AddDbContext<DataContext>(opts =>
            {
                opts.EnableSensitiveDataLogging();
                opts.UseNpgsql(dbConnection, dbOpts =>
                    dbOpts.MigrationsAssembly("WarehouseWebsite.Infrastructure"));
            });

            services.AddQuartz(opts =>
            {
                var jobKey = new JobKey("ItemShippingJob");
                opts.AddJob<ItemShippingJob>(opts =>
                {
                    opts.WithIdentity(jobKey);
                    opts.StoreDurably();
                });
            });
            services.AddQuartzHostedService();
            services.AddTransient<JobStartingHelper>();
            return services;
        }

        public static IServiceProvider DatabaseMigrate(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
            return services;
        }       
    }
}
