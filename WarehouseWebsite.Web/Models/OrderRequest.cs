using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Web.Validation;

namespace WarehouseWebsite.Web.Models
{
    public class OrderRequest
    {
        [NoDuplicateOrderItems]
        [JsonPropertyName("order")]
        public List<OrderItemRequest> Request { get; set; } = null!;

        [JsonPropertyName("customer")]
        public CustomerRequest? CustomerRequest { get; set; }

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

    public class CustomerRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? Address { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public Customer ToCustomer()
        {
            return new Customer
            {
                Name = Name,
                Surname = Surname,
                Address = Address,
                Email = Email
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
