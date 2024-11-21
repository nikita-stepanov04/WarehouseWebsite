namespace WarehouseWebsite.Domain.Models
{
    public class RefreshToken : BaseEntity
    {
        public string? Token { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }

        public bool IsActive => !IsRevoked && DateTime.UtcNow <= Expires;
    }
}
