namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IItemRepository ItemRepository { get; }
        IMissingItemRepository MissingItemRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICustomerRepository CustomerRepository { get; }

        Task SaveAsync();
    }
}
