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

    public class AwaitingOrder : Order { }
}
