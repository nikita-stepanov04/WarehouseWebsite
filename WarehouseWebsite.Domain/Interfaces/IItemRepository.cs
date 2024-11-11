using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Item> GetByIdShortenAsync(Guid id);
        IAsyncEnumerable<Item> GetByFilterAsync(Func<Item, bool> filter);
    }
}
