using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Domain.Filtering;

namespace WarehouseWebsite.Domain.Interfaces.Repositories
{
    public interface IAwaitingOrderRepository : IRepository<AwaitingOrder>
    {
        Task<IEnumerable<AwaitingOrder>> GetAwaitingOrdersAsync(
            FilterParameters<AwaitingOrder> filter, CancellationToken token, bool withItems = false);        
    }
}
