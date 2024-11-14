using WarehouseWebsite.Domain.Models;

namespace WarehouseWebsite.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
