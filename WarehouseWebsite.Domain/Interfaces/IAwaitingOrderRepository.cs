using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Domain.Filtering;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IAwaitingOrderRepository : IRepository<AwaitingOrder>
    {
        Task PlaceOrderToQueueAsync(AwaitingOrder order);

        Task<IEnumerable<AwaitingOrder>> GetAwaitingOrdersAsync(
            FilterParameters<AwaitingOrder> filter, CancellationToken token);
    }
}
