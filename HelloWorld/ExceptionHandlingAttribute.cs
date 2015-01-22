using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace HelloWorld
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //var response = new
            //{
            //    Status = "error",
            //    Message = actionExecutedContext.Exception.Message,
            //};
            var response = new ContactResponse
            {
                Header = new StandardResponseHeader
                {
                    Status = "error",
                    StatusList = new StatusList
                {
                    new Status(StatusType.Error, null, actionExecutedContext.Exception.Message)
                }
                }
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent(response.GetType(), response, new JsonMediaTypeFormatter())
            };

            throw new HttpResponseException(httpResponseMessage);
        }
    }
}