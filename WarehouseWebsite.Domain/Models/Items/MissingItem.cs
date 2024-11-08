namespace WarehouseWebsite.Domain.Models.Items
{
    public class MissingItem : BaseEntity
    {
        public int Missing { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}
