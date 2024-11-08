using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order> 
    {
        Task SetOrderAsDeliveredAsync(Order order);

        Task PlaceOrderAsync(Order order);
        Task PlaceOrderToQueueAsync(Order order);

        Task GetTransitingOrdersForCustomerAsync(Customer customer);
        Task GetTransitedOrdersForCustomerAsync(Customer customer);
        Task GetAwaitingOrdersForCustomerAsync(Customer customer);
    }
}
