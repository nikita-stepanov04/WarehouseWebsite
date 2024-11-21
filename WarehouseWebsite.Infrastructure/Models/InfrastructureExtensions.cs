﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                opts.UseNpgsql(dbConnection, dbOpts =>
                    dbOpts.MigrationsAssembly("WarehouseWebsite.Infrastructure"));
            });
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