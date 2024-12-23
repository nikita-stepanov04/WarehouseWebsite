﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Jobs;
using WarehouseWebsite.Web.Identity;
using WarehouseWebsite.Web.Models;

namespace WarehouseWebsite.Web.Controllers
{
    [Route("api/items")]
    [Authorize(Policy = nameof(Policies.AdminsOnly))]
    public class ItemsController : WarehouseControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _itemService.GetByIdAsync(id);
            return item != null
                ? Ok(item)
                : NotFound();
        }

        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetByFilter(CancellationToken token,
            [FromQuery] int? page = null,
            [FromQuery] int? count = null,
            [FromQuery] ItemCategory? category = null)
        {
            var filterParams = PaginationHelper.FromPagination<Item>(page, count);
            if (category != null)
                filterParams.Filter = i => i.Category == category;

            var items = await _itemService.GetItemsByFilterAsync(filterParams, token);
            return Ok(items);
        }

        [HttpPost()]
        public async Task<IActionResult> Add([FromForm] ItemRequest request)
        {
            var item = request.GetItem();
            using var memoryStream = await request.GetStreamAsync();

            await _itemService.AddItemAsync(item, memoryStream);
            return Ok(new { ItemId = item.Id });
        }
        
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Remove(Guid id,
            [FromServices] JobStartingHelper jobStarter)
        {
            var parameters = new Dictionary<string, string>
            {
                { "itemId", id.ToString() }
            };

            await jobStarter.StartAsync("ItemRemovalJob", parameters);
            return Ok();
        }
        
        [HttpPost("restock/{id:Guid}")]
        public async Task<IActionResult> Restock(Guid id,
            [FromQuery] int addQuantity)
        {
            await _itemService.RestockItemAsync(id, addQuantity);
            return Ok();
        }

        [HttpGet("missing")]
        public async Task<IActionResult> GetMissingByFilter(CancellationToken token,
            [FromQuery] int? page = null,
            [FromQuery] int? count = null,
            [FromQuery] ItemCategory? category = null)
        {
            var filterParams = PaginationHelper.FromPagination<MissingItem>(page, count);
            if (category != null)
                filterParams.Filter = i => i.Item.Category == category;

            var items = await _itemService.GetMissingItemsAsync(filterParams, token);
            return Ok(items);
        }
    }
}
