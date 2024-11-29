using Quartz;
using WarehouseWebsite.Application.Interfaces;

namespace WarehouseWebsite.Infrastructure.Jobs
{
    public class ItemShippingJob : IJob
    {
        private readonly IOrderService _orderService;

        public ItemShippingJob(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _orderService.StartShippingItemsAsync();
        }
    }
}
