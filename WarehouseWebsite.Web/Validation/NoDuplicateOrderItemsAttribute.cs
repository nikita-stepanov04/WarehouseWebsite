using System.ComponentModel.DataAnnotations;
using WarehouseWebsite.Web.Models;

namespace WarehouseWebsite.Web.Validation
{
    public class NoDuplicateOrderItemsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var orderItems = value as List<OrderItemRequest>;
            if (orderItems != null)
            {
                var distinct = orderItems.DistinctBy(oi => oi.ItemId).ToList();
                if (distinct.Count != orderItems.Count)
                {
                    return new ValidationResult("Order contains duplicate items");
                }
            }
            return ValidationResult.Success;
        }
    }
}
