using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Application.Models
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
