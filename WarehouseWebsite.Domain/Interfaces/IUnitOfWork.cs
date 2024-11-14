using System.Data;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository ItemRepository { get; }
        IMissingItemRepository MissingItemRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICustomerRepository CustomerRepository { get; }

        Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
