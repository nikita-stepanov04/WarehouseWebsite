using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IItemRepository? _itemRepository;
        private IMissingItemRepository? _missingItemRepository;
        private IOrderRepository? _orderRepository;
        private IAwaitingOrderRepository? _awaitingOrderRepository;
        private ICustomerRepository? _customerRepository;
        private IImageRepository? _imageRepository;

        private readonly DataContext _dbContext;
        private IDbContextTransaction? _transaction;
        private string? _imageStoreConnection;
        private string? _imageStoreContainer;

        public UnitOfWork(DataContext context) => _dbContext = context;

        public UnitOfWork(DataContext context, IConfiguration config)
        {
            _dbContext = context;
            _imageStoreConnection = config["Azure:ImageConnection"];
            _imageStoreContainer = config["Azure:ImageContainer"];
        }

        public IItemRepository ItemRepository => _itemRepository ??= new ItemRepository(_dbContext);

        public IMissingItemRepository MissingItemRepository => _missingItemRepository ??= new MissingItemRepository(_dbContext);

        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_dbContext);

        public IAwaitingOrderRepository AwaitingOrderRepository => _awaitingOrderRepository ??= new AwaitingOrderRepository(_dbContext);

        public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_dbContext);

        public IImageRepository ImageRepository => _imageRepository ??= new ImageRepository(
            new BlobServiceClient(_imageStoreConnection ?? throw new ArgumentNullException("Image connection is not defined")),
                                  _imageStoreContainer ?? throw new ArgumentNullException("Image store container is not defined"));

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
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();

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
