using System.Text.Json.Serialization;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Domain.Models.Items
{
    public class Item : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public double Weight { get; set; }
        public ItemCategory Category { get; set; }

        [JsonIgnore]
        public bool IsRemoved { get; set; }

        [JsonIgnore]
        public Guid PhotoBlobId { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
