using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Infrastructure.Filtering;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Domain.Interfaces.Repositories;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DataContext context)
            : base(context) { }

        public async Task<IEnumerable<Order>> GetTransitedOrdersAsync(
            FilterParameters<Order> filter, CancellationToken token)
        {
            return await DbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Status == OrderStatus.Transited)
                .WithFilter(filter)
                .ToListAsync(cancellationToken: token);
        }

        public async Task<IEnumerable<Order>> GetTransitingOrdersAsync(
            FilterParameters<Order> filter, CancellationToken token)
        {
            return await DbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o =>  o.Status == OrderStatus.Transiting)
                .WithFilter(filter)
                .ToListAsync(cancellationToken: token);
        }

        public void SetOrderAsTransited(Order order)
        {
            order.Status = OrderStatus.Transited;
            Update(order);
        }
    }
}
