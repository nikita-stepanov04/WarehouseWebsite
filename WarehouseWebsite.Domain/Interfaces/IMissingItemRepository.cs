﻿using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Domain.Interfaces
{
    public interface IMissingItemRepository : IRepository<MissingItem>
    {
        Task<IEnumerable<MissingItem>> GetItemsByFilterAsync(FilterParameters<MissingItem> filter, CancellationToken token);
    }
}
