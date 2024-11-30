using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using WarehouseWebsite.Web.Filters;

namespace WarehouseWebsite.Tests.WebTests
{
    [TestFixture]
    public class ValidateModelAttributeTests
    {
        private ValidateModelAttribute _filter;
        private ActionExecutingContext _actionExecutingContext;

        [SetUp]
        public void SetUp()
        {
            _filter = new ValidateModelAttribute();
            _actionExecutingContext = new ActionExecutingContext(
                actionContext: new ActionContext(
                    new DefaultHttpContext(),
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new ControllerActionDescriptor()
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                new Mock<ControllerBase>().Object
            );
        }

        [Test]
        public void ValidateModelAttributeModelStateIsValidDoesNotSetResult()
        {
            _actionExecutingContext.ModelState.Clear();

            _filter.OnActionExecuting(_actionExecutingContext);

            Assert.That(_actionExecutingContext.Result, Is.Null);
        }
        
        [Test]
        public void ValidateModelAttributeModelStateIsNotValidSetResult()
        {
            _actionExecutingContext.ModelState.AddModelError("Name", "Required");

            _filter.OnActionExecuting(_actionExecutingContext);

            var result = _actionExecutingContext.Result;
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var badResult = result as BadRequestObjectResult;
            Assert.That(badResult!.StatusCode, Is.EqualTo(400));
        }
    }
}
