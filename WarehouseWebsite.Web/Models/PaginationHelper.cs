using WarehouseWebsite.Domain.Filtering;

namespace WarehouseWebsite.Web.Models
{
    public static class PaginationHelper
    {
        public static FilterParameters<T> FromPagination<T>(int? page, int? count)
        {
            var parameters = new FilterParameters<T>();
            if (page.HasValue && count.HasValue)
            {
                parameters.Skip = (page.Value - 1) * count.Value;
            }
            if (count.HasValue)
            {
                parameters.Take = count.Value;
            }
            return parameters;
        }
    }
}
