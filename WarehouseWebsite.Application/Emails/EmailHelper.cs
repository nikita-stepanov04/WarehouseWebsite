using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;
using System.Reflection;

namespace WarehouseWebsite.Application.Emails
{
    public static class EmailHelper
    {
        public static Assembly? EmailViewsAssembly { get; set; }

        public static IServiceCollection MapEmailViesAssembly(
            this IServiceCollection services, Assembly assembly)
        {
            EmailViewsAssembly = assembly;
            return services;
        }

        public static void SetUpFluentEmail(this IServiceCollection services,
            IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("Smtp").Get<FluentEmailSettings>()!;

            services
                .AddFluentEmail(emailSettings.DefaultFromEmail)
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient(emailSettings.Host, emailSettings.Port)
                {
                    Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password),
                    EnableSsl = emailSettings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                });
        }

        public static string GetResourceName(string viewName)
        {
            string? resourceName = EmailViewsAssembly?.GetManifestResourceNames()
                .FirstOrDefault(r => r.EndsWith($"Emails.{viewName}.cshtml"));

            if (resourceName == null) throw new FileNotFoundException($"{viewName} was not found");

            return resourceName;
        }
    }
}
