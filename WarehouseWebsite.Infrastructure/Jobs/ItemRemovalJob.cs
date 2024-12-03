using Quartz;
using WarehouseWebsite.Application.Interfaces;

namespace WarehouseWebsite.Infrastructure.Jobs
{
    internal class ItemRemovalJob : IJob
    {
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;

        public ItemRemovalJob(IItemService itemService, IOrderService orderService)
        {
            _itemService = itemService;
            _orderService = orderService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var itemId = Guid.Parse(context.MergedJobDataMap.GetString("itemId")!);

            var item = await _itemService.GetByIdAsync(itemId);

            if (item == null) return;

            await _itemService.RemoveItemByIdAsync(itemId);
            await _orderService.RemoveDeletedItemFromAwaitingOrders(item);
        }
    }
}
