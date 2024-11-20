using System.ComponentModel.DataAnnotations;

namespace WarehouseWebsite.Web.Models
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public required string Password { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 1)]
        public required string Name { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 1)]
        public required string Surname { get; set; }
    }
}
