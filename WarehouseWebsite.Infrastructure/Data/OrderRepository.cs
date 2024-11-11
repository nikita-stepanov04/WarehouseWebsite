using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) 
            : base(context) { }

        public Task GetAwaitingOrdersForCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task GetTransitedOrdersForCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task GetTransitingOrdersForCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task PlaceOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task PlaceOrderToQueueAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task SetOrderAsDeliveredAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
