using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Domain.Interfaces.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(
            FilterParameters<OrderItem> filter, CancellationToken token);
    }
}
