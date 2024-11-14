using WarehouseWebsite.Application.Models;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Application.Interfaces
{
    internal interface IOrderService
    {
        Task SetOrderAsTransitedByIdAsync(Guid id);
        Task StartShippingItemsAsync();

        Task PlaceOrderAsync(OrderDTO order, Guid customerId);

        Task<IEnumerable<OrderDTO>> GetTransitingOrdersAsync(FilterParameters<Order> filter, CancellationToken token);
        Task<IEnumerable<OrderDTO>> GetTransitedOrdersAsync(FilterParameters<Order> filter, CancellationToken token);
        Task<IEnumerable<OrderDTO>> GetAwaitingOrdersAsync(FilterParameters<AwaitingOrder> filter, CancellationToken token);
    }
}
