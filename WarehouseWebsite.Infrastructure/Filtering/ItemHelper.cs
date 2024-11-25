using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Infrastructure.Filtering
{
    public static class ItemHelper
    {
        public static IQueryable<Item> SelectWithoutDescription(this IQueryable<Item> items)
        {
            return items.Select(i => new Item 
            {
                Id = i.Id,
                Name = i.Name,
                Quantity = i.Quantity,
                Weight = i.Weight,
                Category = i.Category,
                Price = i.Price,
                PhotoBlobId = i.PhotoBlobId
            });
        }
        
        public static IQueryable<MissingItem> SelectMissingWithoutDescription(
            this IQueryable<MissingItem> items)
        {
            return items.Select(i => new MissingItem
            {
                Missing = i.Missing,
                Item = new Item
                {
                    Id = i.Item.Id,
                    Name = i.Item.Name,
                    Quantity = i.Item.Quantity,
                    Weight = i.Item.Weight,
                    Category = i.Item.Category,
                    Price = i.Item.Price,
                    PhotoBlobId = i.Item.PhotoBlobId
                }
            });
        }
    }
}
