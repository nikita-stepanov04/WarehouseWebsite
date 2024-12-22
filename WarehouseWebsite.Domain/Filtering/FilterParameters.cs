using System.Linq.Expressions;

namespace WarehouseWebsite.Domain.Filtering
{
    public class FilterParameters<T>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public Expression<Func<T, bool>>? Filter { get; set; }
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDescending { get; set; }
    }
}
