using System.ComponentModel.DataAnnotations;

namespace WarehouseWebsite.Domain.Models.Items
{
    public class Item : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        public int Quantity { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        [Range(0.01, 10_000_000)]
        public decimal Price { get; set; }

        [Range(0.1, 1000)]
        public double Weight { get; set; }

        public Guid SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        public ItemCategory Category { get; set; }
    }
}
