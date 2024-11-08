using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Domain.Models.Orders
{
    public class OrderItem : BaseEntity
    {
        public Guid ItemId { get; set; }
        public Guid OrderId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Item? Item { get; set; }
        public Order? Order { get; set; }
    }
}
