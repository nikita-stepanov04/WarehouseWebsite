﻿using WarehouseWebsite.Domain.Filtering;

namespace WarehouseWebsite.Infrastructure.Filtering
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WithFilter<T>(
            this IQueryable<T> query, FilterParameters<T> parameters)
        {
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
