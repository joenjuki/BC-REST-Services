using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HelloWorld
{
    public class AuthenticatorAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization.Scheme == "Bearer")
            {
                var tokenString = authorization.Parameter;
                try
                {
                    var token = TokenHelper.DecodeToken(tokenString);
                    if (token.Expires < DateTime.UtcNow)
                    {
                        throw new HttpResponseException(HttpStatusCode.Forbidden);
                    }
                }
                catch (Exception)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
            }

            base.OnAuthorization(actionContext);
        }
    }
}