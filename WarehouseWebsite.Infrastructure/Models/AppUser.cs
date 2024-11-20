using Microsoft.AspNetCore.Identity;
using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Domain.Models.Customers;

namespace WarehouseWebsite.Infrastructure.Models
{
    public class AppUser : IdentityUser
    {
        public Guid CustomerId { get; set; }
        public Guid? RefreshTokenId { get; set; }

        public RefreshToken? RefreshToken { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
    }
}
