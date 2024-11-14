using WarehouseWebsite.Domain.Models.Customers;

namespace WarehouseWebsite.Application.Interfaces
{
    public interface ICustomerService
    {
        Task AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(Guid id);
        Task RemoveByIdAsync(Guid id);
        Task UpdateAsync(Customer customer);
    }
}
