using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Jobs;
using WarehouseWebsite.Web.Identity;
using WarehouseWebsite.Web.Models;

namespace WarehouseWebsite.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(
            [FromBody] OrderRequest request)
        {
            Guid customerId = UserCustomerId;
            var order = request.GetOrder();

            try
            {
                var id = await _orderService.PlaceOrderAsync(order, customerId);
                return Ok(new { OrderId = id });
            } 
            catch (Exception e)
            {
                string message = e switch
                {
                    InvalidOperationException => "Order is invalid",
                    OperationCanceledException => "Failed to place order, try again later",
                    _ => "Unexpected error while processing request"
                };
                return BadRequest(new { Message = message });
            }            
        }

        [HttpGet()]
        public async Task<IActionResult> GetOrders(CancellationToken token,
            [FromQuery] OrderStatus status,
            [FromQuery] int? page = null,
            [FromQuery] int? count = null)
        {
            bool isAdmin = User.IsInRole(nameof(Roles.Admin));
            IEnumerable<Order> orders = [];

            if (status == OrderStatus.Awaiting)
            {
                var filterParams = PaginationHelper.FromPagination<AwaitingOrder>(page, count);
                filterParams.Filter = !isAdmin
                    ? o => o.CustomerId == UserCustomerId
                    : default;
                orders = await _orderService.GetAwaitingOrdersAsync(filterParams, token);
            }
            else if (status == OrderStatus.Transited || status == OrderStatus.Transiting)
            {
                var filterParams = PaginationHelper.FromPagination<Order>(page, count);
                filterParams.Filter = !isAdmin
                    ? o => o.CustomerId == UserCustomerId
                    : default;
                orders = status switch
                {
                    OrderStatus.Transited => await _orderService.GetTransitedOrdersAsync(filterParams, token),
                    OrderStatus.Transiting => await _orderService.GetTransitingOrdersAsync(filterParams, token),
                    _ => []
                };
            }
            return Ok(orders);
        }

        [HttpPost("set-transited/{orderId:guid}")]
        //[Authorize(Policy = nameof(Policies.AdminsOnly))]
        public async Task<IActionResult> SetOrderAsTransited(Guid orderId)
        {
            await _orderService.SetOrderAsTransitedByIdAsync(orderId);
            return Ok();
        }

        [HttpGet("start-shipping")]
        //[Authorize(Policy = nameof(Policies.AdminsOnly))]
        public async Task<IActionResult> StartShipping(
            [FromServices] JobStartingHelper jobStarter)
        {
            await jobStarter.StartAsync("ItemShippingJob");
            return Ok();
        }

        private Guid UserCustomerId => Guid.Parse(User.FindFirstValue("CustomerId")!);
    }
}
