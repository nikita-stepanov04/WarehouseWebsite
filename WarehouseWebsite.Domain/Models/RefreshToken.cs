namespace WarehouseWebsite.Domain.Models
{
    public class RefreshToken : BaseEntity
    {
        public required string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }

        public bool IsActive => !IsRevoked && DateTime.UtcNow <= Expires;
    }
}
