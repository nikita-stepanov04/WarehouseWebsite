using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class MissingItemRepository : Repository<MissingItem>, IMissingItemRepository
    {
        public MissingItemRepository(DataContext context)
            : base(context) { }

        public void AddOrUpdateMissingItemAsync(Item item, int addQuantity)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<MissingItem> GetByFilterAsync(Func<MissingItem, bool> filter)
        {
            throw new NotImplementedException();
        }
    }
}
