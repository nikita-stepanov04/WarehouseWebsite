using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(DataContext context) 
            : base(context) { }
    }
}
