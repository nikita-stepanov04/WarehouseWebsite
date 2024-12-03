using WarehouseWebsite.Domain.Models.Emails;

namespace WarehouseWebsite.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendRazorAsync<T>(EmailMetadata<T> emailMetadata);
    }
}
