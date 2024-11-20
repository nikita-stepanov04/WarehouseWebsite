using System.ComponentModel.DataAnnotations;

namespace WarehouseWebsite.Web.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public required string Password { get; set; }
    }
}
