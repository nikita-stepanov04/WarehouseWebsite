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
                .SelectMissingWithoutDescription()
                .WithFilter(filter)
                .ToListAsync(cancellationToken: token);
        }

        public async Task AddToMissing(IEnumerable<MissingItem> addToMissingList, CancellationToken token)
        {
            var ids = addToMissingList.Select(mi => mi.ItemId).ToList();
            var filter = new FilterParameters<MissingItem>
            {
                Filter = mi => ids.Contains(mi.ItemId)
            };

            var missingList = await DbContext.MissingItems
                .WithFilter(filter)
                .ToListAsync(token);

            foreach (var addToMissingItem in addToMissingList)
            {
                var missingItem = missingList.FirstOrDefault(mi => mi.ItemId == addToMissingItem.ItemId);

                if (missingItem == null)
                    DbContext.MissingItems.Add(addToMissingItem);
                else
                    missingItem.Missing += addToMissingItem.Missing;                
            }
        }

        public async Task<MissingItem?> GetByItemIdNotPopulated(Guid id)
        {
            return await DbContext.MissingItems.FirstOrDefaultAsync(mi => mi.ItemId == id);
        }
    }
}
