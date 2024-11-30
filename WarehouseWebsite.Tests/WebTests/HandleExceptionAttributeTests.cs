using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using WarehouseWebsite.Web.Filters;

namespace WarehouseWebsite.Tests.WebTests
{
    [TestFixture]
    public class HandleExceptionAttributeTests
    {
        private HandleExceptionAttribute _filter;
        private ExceptionContext _exceptionContext;

        private string _invalidOperationMessage = "Request is invalid";
        private string _operationCancelledMessage = "Failed to handle request, try again later";
        private string _unexpectedExceptionMessage = "Unexpected error while processing request";

        [SetUp]
        public void SetUp()
        {
            _filter = new HandleExceptionAttribute();

            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new Microsoft.AspNetCore.Routing.RouteData(),
                new ControllerActionDescriptor()
            );

            _exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>());
        }

        [Test]
        public void HandleExceptionAttributeOnInvalidOperationExceptionSetsBadResult()
        {
            _exceptionContext.Exception = new InvalidOperationException();

            _filter.OnException(_exceptionContext);

            var result = _exceptionContext.Result;
            Assert.That(result, Is.TypeOf<JsonResult>());

            var jsonResult = result as JsonResult;
            Assert.That(jsonResult!.StatusCode, Is.EqualTo(400));

            string resultJson = JsonConvert.SerializeObject(jsonResult.Value);
            var messageResult = JsonConvert.DeserializeAnonymousType(resultJson, new { Message = "" })!;

            Assert.That(_invalidOperationMessage, Is.EqualTo(messageResult.Message));
        }
        
        [Test]
        public void HandleExceptionAttributeOnOperationCancelledExceptionSetsBadResult()
        {
            _exceptionContext.Exception = new OperationCanceledException();

            _filter.OnException(_exceptionContext);

            var result = _exceptionContext.Result;
            Assert.That(result, Is.TypeOf<JsonResult>());

            var jsonResult = result as JsonResult;
            Assert.That(jsonResult!.StatusCode, Is.EqualTo(400));

            string resultJson = JsonConvert.SerializeObject(jsonResult.Value);
            var messageResult = JsonConvert.DeserializeAnonymousType(resultJson, new { Message = "" })!;

            Assert.That(_operationCancelledMessage, Is.EqualTo(messageResult.Message));
        }
      
        [Test]
        public void HandleExceptionAttributeOnExceptionSetsInternalServerError()
        {
            _exceptionContext.Exception = new Exception();

            _filter.OnException(_exceptionContext);

            var result = _exceptionContext.Result;
            Assert.That(result, Is.TypeOf<JsonResult>());

            var jsonResult = result as JsonResult;
            Assert.That(jsonResult!.StatusCode, Is.EqualTo(500));

            string resultJson = JsonConvert.SerializeObject(jsonResult.Value);
            var messageResult = JsonConvert.DeserializeAnonymousType(resultJson, new { Message = "" })!;

            Assert.That(_unexpectedExceptionMessage, Is.EqualTo(messageResult.Message));
        }
        
        [Test]
        public void HandleExceptionAttributeOnNoExceptionDoNotSetResult()
        {
            _exceptionContext.Result = new OkResult();

            _filter.OnException(_exceptionContext);

            Assert.That(_exceptionContext.Result, Is.TypeOf<JsonResult>());
        }
    }
}
