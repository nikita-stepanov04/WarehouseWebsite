using FluentEmail.Core;
using WarehouseWebsite.Application.Emails;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Models.Emails;

namespace WarehouseWebsite.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmailFactory _emailFactory;

        public EmailService(IFluentEmailFactory emailFactory)
        {
            _emailFactory = emailFactory;
        }

        public async Task SendRazorAsync<T>(EmailMetadata<T> email)
        {
            if (email.ViewName == null) throw new ArgumentNullException(nameof(email.ViewName));
            if (email.Model == null) throw new ArgumentNullException(nameof(email.Model));

            await _emailFactory.Create()
                .To(email.ToAddress)
                .Subject(email.Subject)
                .UsingTemplateFromEmbedded(
                    path: EmailHelper.GetResourceName(email.ViewName),
                    model: email.Model,
                    assembly: EmailHelper.EmailViewsAssembly)
                .SendAsync();
        }
    }
}
