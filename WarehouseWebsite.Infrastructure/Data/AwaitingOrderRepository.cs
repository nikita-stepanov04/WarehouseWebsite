using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Infrastructure.Filtering;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Models;

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

        public async Task PlaceOrderToQueueAsync(AwaitingOrder order)
        {
            await DbContext.AwaitingOrders.AddAsync(order);
        }
    }
}
