using MediatR;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.DomainEvents;
using WarehouseWebsite.Domain.Models.Emails;
using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Application.EventHandlers
{
    public class ItemRemovedFromOrderEventHandler
        : INotificationHandler<ItemRemovedFromOrderEvent>
    {
        private readonly IEmailService _emailService;

        public ItemRemovedFromOrderEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(ItemRemovedFromOrderEvent notification, CancellationToken _)
        {
            await _emailService.SendRazorAsync(new EmailMetadata<Item>
            {
                ToAddress = notification.Order.Customer.Email!,
                Subject = "Item was removed from your order",
                ViewName = "ItemRemovedEmail",
                Model = notification.Item
            });
        }
    }
}
