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

        public UnitOfWork(DataContext context)
        {
            _dbContext = context;
        }

        public IItemRepository ItemRepository => _itemRepository ??= new ItemRepository(_dbContext);

        public IMissingItemRepository MissingItemRepository => _missingItemRepository ??= new MissingItemRepository(_dbContext);

        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_dbContext);

        public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_dbContext);

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
