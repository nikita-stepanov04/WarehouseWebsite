using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DataContext context) 
            : base(context) { }
    }
}
