using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Domain.Interfaces.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        void UpdateQuantity(Item item);
        Task<Item?> GetByIdShortenAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsByFilterAsync(FilterParameters<Item> filter, CancellationToken token);
    }
}
