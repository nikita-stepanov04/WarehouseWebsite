using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected DataContext DbContext { get; }

        public Repository(DataContext context)
        {
            DbContext = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
        }
    }
}
