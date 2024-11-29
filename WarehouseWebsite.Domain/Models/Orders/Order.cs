using WarehouseWebsite.Domain.Models.Customers;

namespace WarehouseWebsite.Domain.Models.Orders
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; } = null!;
        public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class AwaitingOrder : Order 
    {
        public AwaitingOrder() { }

        public AwaitingOrder(Order order)
        {
            CustomerId = order.CustomerId;
            OrderTime = order.OrderTime;
            Status = OrderStatus.Awaiting;
            TotalPrice = order.TotalPrice;
            Customer = order.Customer;
            OrderItems = order.OrderItems;
        }

        public Order ToOrder()
        {
            return new Order
            {
                CustomerId = CustomerId,
                OrderTime = OrderTime,
                Status = OrderStatus.Transiting,
                TotalPrice = TotalPrice,
                Customer = Customer,
                OrderItems = OrderItems,
            };
        }
    }
}
