using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WarehouseWebsite.Web.Filters
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            (HttpStatusCode statusCode, string message) = context.Exception switch
            {
                InvalidOperationException => (HttpStatusCode.BadRequest, "Request is invalid"),
                OperationCanceledException => (HttpStatusCode.BadRequest, "Failed to handle request, try again later"),
                _ => (HttpStatusCode.InternalServerError, "Unexpected error while processing request")
            };

            context.Result = new JsonResult(new { Message = message })
            {
                StatusCode = (int)statusCode
            };
        }
    }
}
