using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(DataContext context) 
            : base(context) { }

        public IAsyncEnumerable<Item> GetByFilterAsync(Func<Item, bool> filter)
        {
            return DbContext.Items
                .Where(i => filter(i))
                .Select(i => SelectWithoutDescription(i))
                .AsAsyncEnumerable();
        }

        public async Task<Item> GetByIdShortenAsync(Guid id)
        {
            return await DbContext.Items
                .Where(i => i.Id == id)
                .Select(i => SelectWithoutDescription(i))
                .FirstAsync();
        }

        private Item SelectWithoutDescription(Item i)
        {
            return new Item
            {
                Id = i.Id,
                Name = i.Name,
                Quantity = i.Quantity,
                Weight = i.Weight,
                Category = i.Category,
                Price = i.Price,
                Supplier = i.Supplier,
                SupplierId = i.SupplierId
            };
        }
    }
}
