using System.ComponentModel.DataAnnotations;

namespace WarehouseWebsite.Web.Identity
{
    public class JwtSettings
    {
        [Required]
        public string Key { get; set; } = null!;

        [Required]
        public int AccessTokenExpirationMinutes { get; set; }

        [Required]
        public int RefreshTokenExpirationDays { get; set; }
    }
}
