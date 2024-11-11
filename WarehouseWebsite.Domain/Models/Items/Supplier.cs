using System.ComponentModel.DataAnnotations;

namespace WarehouseWebsite.Domain.Models.Items
{
    public class Supplier : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public IEnumerable<Item>? Items { get; set; }
    }
}