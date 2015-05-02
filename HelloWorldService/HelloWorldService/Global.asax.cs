using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using HelloWorldService.Controllers;

namespace HelloWorldService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionHandlingAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new LoggingAttribute());
        }
    }
}
