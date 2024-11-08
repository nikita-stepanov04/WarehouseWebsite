using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IMissingItemRepository : IRepository<MissingItem>
    {
        void AddOrUpdateMissingItemAsync(Item item, int addQuantity);

        IAsyncEnumerable<MissingItem> GetByFilterAsync(Func<MissingItem, bool> filter);
    }
}
