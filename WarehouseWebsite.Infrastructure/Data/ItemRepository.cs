using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Interfaces;
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
            return await DbContext.Items
                .WithFilter(filter)
                .Select(i => ItemHelper.SelectWithoutDescription(i))
                .ToListAsync(cancellationToken: token);
        }

        public async Task<Item?> GetByIdShortenAsync(Guid id)
        {
            return await DbContext.Items
                .Where(i => i.Id == id)
                .Select(i => ItemHelper.SelectWithoutDescription(i))
                .FirstAsync();
        }
    }
}
