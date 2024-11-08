using System.ComponentModel.DataAnnotations;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Domain.Models.Customers
{
    public class Customer : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; } = null!;

        [StringLength(100)]
        public string? Address { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public IEnumerable<Order>? Orders { get; set; }
    }
}
