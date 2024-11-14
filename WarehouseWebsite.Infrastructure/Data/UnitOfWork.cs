using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IItemRepository? _itemRepository;
        private IMissingItemRepository? _missingItemRepository;
        private IOrderRepository? _orderRepository;
        private ICustomerRepository? _customerRepository;

        private readonly DataContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(DataContext context)
        {
            _dbContext = context;
        }

        public IItemRepository ItemRepository => _itemRepository ??= new ItemRepository(_dbContext);

        public IMissingItemRepository MissingItemRepository => _missingItemRepository ??= new MissingItemRepository(_dbContext);

        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_dbContext);

        public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_dbContext);

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel,
            CancellationToken cancellationToken)
        {
            _transaction = await _dbContext.Database
                .BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                _transaction = null;
            }
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
