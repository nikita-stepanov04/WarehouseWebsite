using System.Text.Json.Serialization;
using WarehouseWebsite.Application.Emails;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Web.Identity;

namespace WarehouseWebsite.Web.Configure
{
    public static class ConfigureServices
    {
        public static IServiceCollection SetUpOptions(this IServiceCollection services)
        {
            services.AddOptions<JwtSettings>()
                .BindConfiguration("Jwt")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AzureSettings>()
                .BindConfiguration("Azure")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<FluentEmailSettings>()
                .BindConfiguration("Smtp")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public static IMvcBuilder SetUpJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            return builder;
        }
    }
}
