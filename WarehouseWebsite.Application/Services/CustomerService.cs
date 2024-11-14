using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Customers;

namespace WarehouseWebsite.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = unitOfWork.CustomerRepository;
        }

        public async Task AddAsync(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null)
            {
                _customerRepository.Remove(customer);
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _customerRepository.Update(customer);
            await _unitOfWork.SaveAsync();
        }
    }
}
