using System.Data;
using WarehouseWebsite.Domain.Interfaces.Repositories;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository ItemRepository { get; }
        IMissingItemRepository MissingItemRepository { get; }
        IOrderRepository OrderRepository { get; }
        IAwaitingOrderRepository AwaitingOrderRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IImageRepository ImageRepository { get; }

        Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync();
        Task SaveAsync(CancellationToken cancellationToken);
        Task SaveAsync();
    }
}
