using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Infrastructure.Filtering;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Domain.Interfaces.Repositories;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class AwaitingOrderRepository : Repository<AwaitingOrder>, IAwaitingOrderRepository
    {
        public AwaitingOrderRepository(DataContext context)
            : base(context) { }

        public async Task<IEnumerable<AwaitingOrder>> GetAwaitingOrdersAsync(
            FilterParameters<AwaitingOrder> filter, CancellationToken token)
        {
            return await DbContext.AwaitingOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .WithFilter(filter)
                .ToListAsync(cancellationToken: token);
        }
    }
}
