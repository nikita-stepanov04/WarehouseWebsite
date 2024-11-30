using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Web.Validation;

namespace WarehouseWebsite.Web.Models
{
    public class OrderRequest
    {
        [NoDuplicateOrderItems]
        [JsonPropertyName("order")]
        public List<OrderItemRequest> Request { get; set; } = null!;

        public Order GetOrder()
        {
            return new Order
            {
                OrderItems = Request.Select(oir => new OrderItem()
                {
                    ItemId = oir.ItemId,
                    Quantity = oir.Quantity
                }).ToList()
            };
        }
    }

    public class OrderItemRequest
    {
        public Guid ItemId { get; set; }

        [Required]
        [Range(1, 999)]
        public int Quantity { get; set; }
    }
}
