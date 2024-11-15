using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Application.Interfaces
{
    public interface IItemService
    {
        Task RestockItemAsync(Guid id, int addQuantity);
        Task AddItemAsync(Item item, Stream image);
        Task RemoveItemByIdAsync(Guid id);
        Task<Item?> GetByIdAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsByFilterAsync(FilterParameters<Item> filter, CancellationToken token);
        Task<IEnumerable<MissingItem>> GetMissingItemsAsync(FilterParameters<MissingItem> filter, CancellationToken token);
    }
}
