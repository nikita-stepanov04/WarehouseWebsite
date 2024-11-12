using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Infrastructure.Filtering
{
    public static class ItemHelper
    {
        public static Item SelectWithoutDescription(Item i)
        {
            return new Item
            {
                Id = i.Id,
                Name = i.Name,
                Quantity = i.Quantity,
                Weight = i.Weight,
                Category = i.Category,
                Price = i.Price
            };
        }
    }
}
