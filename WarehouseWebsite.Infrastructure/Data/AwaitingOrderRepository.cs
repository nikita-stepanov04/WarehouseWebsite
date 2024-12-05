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
            FilterParameters<AwaitingOrder> filter,
            CancellationToken token,
            bool withItems = true,
            bool withCustomer = false)
        {
            IQueryable<AwaitingOrder> query = DbContext.AwaitingOrders;

            if (withItems)
                query = query.Include(o => o.OrderItems).ThenInclude(oi => oi.Item);
            else
                query = query.Include(o => o.OrderItems);

            if (withCustomer)
                query = query.Include(o => o.Customer);

            query = query.WithFilter(filter);

            return await query.ToListAsync(cancellationToken: token);
        }
    }
}
