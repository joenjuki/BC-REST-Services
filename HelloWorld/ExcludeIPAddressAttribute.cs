using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace HelloWorld
{
    public class ExcludeIPAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var address = HttpContext.Current.Request.UserHostAddress;
            
            // SSL-Check
            //if (actionContext.Request.RequestUri.Scheme != "https")
            //{
            //    throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            //}

            if (address == "::1")
            {
               // throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}