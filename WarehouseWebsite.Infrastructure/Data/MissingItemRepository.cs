using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Filtering;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class MissingItemRepository : Repository<MissingItem>, IMissingItemRepository
    {
        public MissingItemRepository(DataContext context)
            : base(context) { }

        public async Task<IEnumerable<MissingItem>> GetItemsByFilterAsync(
            FilterParameters<MissingItem> filter, CancellationToken token)
        {
            return await DbContext.MissingItems
                .Include(mi => mi.Item)
                .Select(mi => new MissingItem()
                {
                    Missing = mi.Missing,
                    Item = ItemHelper.SelectWithoutDescription(mi.Item)
                })
                .WithFilter(filter)
                .ToListAsync(cancellationToken: token);
        }
    }
}
