using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Filtering;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(DataContext context) 
            : base(context) { }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(
            FilterParameters<OrderItem> filter, CancellationToken token)
        {
            return await DbContext.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Customer)
                .Include(oi => oi.AwaitingOrder)
                    .ThenInclude(ao => ao.Customer)
                .WithFilter(filter)
                .ToListAsync();
        }
    }
}
