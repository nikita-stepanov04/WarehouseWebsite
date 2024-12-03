using Azure.Storage.Blobs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Data;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IItemRepository? _itemRepository;
        private IMissingItemRepository? _missingItemRepository;
        private IOrderRepository? _orderRepository;
        private IOrderItemRepository? _orderItemRepository;
        private IAwaitingOrderRepository? _awaitingOrderRepository;
        private ICustomerRepository? _customerRepository;
        private IImageRepository? _imageRepository;

        private readonly DataContext _dbContext;

        private IMediator? _mediator;
        private IDbContextTransaction? _transaction;
        private AzureSettings? _azureSettings;

        public UnitOfWork(DataContext context) => _dbContext = context;

        public UnitOfWork(DataContext context, IMediator mediator, IOptions<AzureSettings> azureSettings)
        {
            _dbContext = context;
            _mediator = mediator;
            _azureSettings = azureSettings.Value;
        }

        public IItemRepository ItemRepository => _itemRepository ??= new ItemRepository(_dbContext);

        public IMissingItemRepository MissingItemRepository => _missingItemRepository ??= new MissingItemRepository(_dbContext);

        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_dbContext);

        public IOrderItemRepository OrderItemRepository => _orderItemRepository ??= new OrderItemRepository(_dbContext);

        public IAwaitingOrderRepository AwaitingOrderRepository => _awaitingOrderRepository ??= new AwaitingOrderRepository(_dbContext);

        public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_dbContext);

        public IImageRepository ImageRepository =>
            _imageRepository ??= new ImageRepository(
                new BlobServiceClient(_azureSettings!.ImageConnection),
                _azureSettings!.ImageContainer
            );

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

        public async Task SaveAsync() => await SaveAsync(default);

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            if (_mediator != null)
            {
                await _mediator.DispatchDomainEventsAsync(_dbContext);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void DetachItems()
        {
            foreach (var entity in _dbContext.ChangeTracker.Entries<Item>().ToList())
            {
                entity.State = EntityState.Detached;
            }
        }

        public void ClearContext()
        {
            _dbContext.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
