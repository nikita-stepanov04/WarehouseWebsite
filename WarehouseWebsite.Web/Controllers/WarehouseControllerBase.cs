using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WarehouseWebsite.Web.Filters;

namespace WarehouseWebsite.Web.Controllers
{
    [ApiController]
    [ValidateModel]
    [HandleException]
    public class WarehouseControllerBase : ControllerBase 
    {
        private string customerId = "CustomerId";
        public Guid UserCustomerId => Guid.Parse(User.FindFirstValue(customerId)
            ?? throw new ArgumentNullException(customerId, "User was not authenticated"));
    }
}
