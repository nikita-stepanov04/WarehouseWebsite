using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Domain.Filtering;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void SetOrderAsTransited(Order order);

        Task PlaceOrderAsync(Order order);

        Task<IEnumerable<Order>> GetTransitingOrdersAsync(FilterParameters<Order> filter, CancellationToken token);

        Task<IEnumerable<Order>> GetTransitedOrdersAsync(FilterParameters<Order> filter, CancellationToken token);
    }
}
