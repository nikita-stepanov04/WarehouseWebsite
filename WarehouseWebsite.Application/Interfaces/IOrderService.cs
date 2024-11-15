using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Application.Interfaces
{
    internal interface IOrderService
    {
        Task SetOrderAsTransitedByIdAsync(Guid id);
        Task StartShippingItemsAsync();

        Task PlaceOrderAsync(Order order, Guid customerId);

        Task<IEnumerable<Order>> GetTransitingOrdersAsync(FilterParameters<Order> filter, CancellationToken token);
        Task<IEnumerable<Order>> GetTransitedOrdersAsync(FilterParameters<Order> filter, CancellationToken token);
        Task<IEnumerable<Order>> GetAwaitingOrdersAsync(FilterParameters<AwaitingOrder> filter, CancellationToken token);
    }
}
