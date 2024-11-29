using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Domain.Interfaces.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetItemsByIdsAsNoTracking(IEnumerable<Guid> ids, CancellationToken token);
        void UpdateQuantity(Item item);
        Task<Item?> GetByIdShortenAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsByFilterAsync(FilterParameters<Item> filter, CancellationToken token);
    }
}
