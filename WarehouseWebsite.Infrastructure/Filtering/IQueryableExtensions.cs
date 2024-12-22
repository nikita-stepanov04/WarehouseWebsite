using Microsoft.CodeAnalysis.CSharp.Syntax;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models;

namespace WarehouseWebsite.Infrastructure.Filtering
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WithFilter<T>(
            this IQueryable<T> query, FilterParameters<T> parameters)
            where T : BaseEntity
        {
            if (parameters.OrderBy != null)
            {
                query = query.OrderBy(parameters.OrderBy);
            }
            else if (parameters.OrderByDescending != null)
            {
                query = query.OrderByDescending(parameters.OrderByDescending);
            }
            else
            {
                query = query.OrderBy(i => i.Id);
            }
            if (parameters.Filter != null)
            {
                query = query.Where(parameters.Filter);
            }
            if (parameters.Skip > 0)
            {
                query = query.Skip(parameters.Skip);
            }
            if (parameters.Take > 0)
            {
                query = query.Take(parameters.Take);
            }
            return query;
        }
    }
}
