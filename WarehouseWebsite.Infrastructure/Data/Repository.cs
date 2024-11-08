using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    internal class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        public Task AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
