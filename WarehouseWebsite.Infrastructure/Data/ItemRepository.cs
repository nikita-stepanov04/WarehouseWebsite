using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Filtering;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(DataContext context) 
            : base(context) { }

        public async Task<IEnumerable<Item>> GetItemsByFilterAsync(
            FilterParameters<Item> filter, CancellationToken token)
        {
            List<Item> items = await DbContext.Items
                .Where(i => !i.IsRemoved)
                .WithFilter(filter)
                .SelectWithoutDescription()
                .ToListAsync(cancellationToken: token);
            return items;
        }

        public override void Remove(Item item)
        {
            item.IsRemoved = true;
        }

        public void UpdateQuantity(Item item)
        {
            DbContext.Entry(item).State = EntityState.Unchanged;
            DbContext.Entry(item).Property(i => i.Quantity).IsModified = true;
        }

        public async Task<Item?> GetByIdShortenAsync(Guid id)
        {
            return await DbContext.Items
                .Where(i => i.Id == id)
                .SelectWithoutDescription()
                .FirstAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsByIdsAsNoTracking(IEnumerable<Guid> ids, CancellationToken token)
        {
            var filter = new FilterParameters<Item>
            {
                Filter = i => ids.Contains(i.Id)
            };
            return await DbContext.Items
                .AsNoTracking()
                .WithFilter(filter)
                .SelectWithoutDescription()
                .ToListAsync(cancellationToken: token);
        }
    }
}
