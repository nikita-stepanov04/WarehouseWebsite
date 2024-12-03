using Microsoft.AspNetCore.Mvc;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Models.Emails;
using WarehouseWebsite.Domain.Models.Items;

namespace WarehouseWebsite.Web.Controllers
{
    [Route("api/email")]
    public class TestController : WarehouseControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> SendEmail(
            IEmailService emailService, IItemService itemService)
        {
            await emailService.SendRazorAsync(new EmailMetadata<Item>()
            {
                ToAddress = "test@mail.com",
                Subject = "Test email with razor",
                ViewName = "ItemRemovedEmail",
                Model = await itemService.GetByIdAsync(Guid.Parse("019388e6-5808-76ba-9c66-c945a42f9459"))
            });
            return Ok();
        }
    }
}
