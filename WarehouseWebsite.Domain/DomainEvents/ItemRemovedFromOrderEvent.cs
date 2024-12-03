using MediatR;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Domain.DomainEvents
{
    public record ItemRemovedFromOrderEvent(Item Item, Order Order)
        : INotification { }
}
