﻿namespace WarehouseWebsite.Domain.Filtering
{
    public class FilterParameters<T>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public Func<T, bool>? Filter { get; set; }
    }
}