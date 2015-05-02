using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWorldService.Models;
using Newtonsoft.Json;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;

namespace HelloWorldService.Controllers
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        private System.Diagnostics.Stopwatch stopwatch;

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
           // System.Web.HttpContext.

            var currentRequest = actionContext.Request;
            stopwatch = System.Diagnostics.Stopwatch.StartNew();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            stopwatch.Stop();
            var milliseconds = stopwatch.ElapsedMilliseconds;
            System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/Logger.txt"),
                string.Format("{0} : {1} {2} Elapsed={3}\n",
                System.DateTime.Now, 
                actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                actionExecutedContext.ActionContext.Request.Method,
                stopwatch.Elapsed));

            base.OnActionExecuted(actionExecutedContext);
        }
    }

    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var response = new
            {
                Status = "MyError",
                Message = actionExecutedContext.Exception.Message,
            };
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent(response.GetType(), response, new JsonMediaTypeFormatter())
            };
            throw new HttpResponseException(httpResponseMessage);
        }
    }

    public class ContactsController : ApiController
    {
        private static int nextId = 100;
        private static List<Contact> contacts = new List<Contact>();

        // GET: api/Contacts
        public IEnumerable<Contact> Get()
        {
            return contacts;
        }

        // GET: api/Contacts/5
        public Contact Get(int id)
        {
            var contact = contacts.SingleOrDefault(t => t.CONTACTSID == id);
            return contact;
        }

        // POST: api/Contacts
        public HttpResponseMessage Post([FromBody]Contact value)
        {
            if (value == null)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            value.CONTACTSID = nextId++;
            contacts.Add(value);

            var result = new { Id = value.CONTACTSID, Candy = true };

            var newJson = JsonConvert.SerializeObject(result);

            var postContent = new StringContent(newJson, System.Text.Encoding.UTF8, "application/json");

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = postContent
            };
        }

        // PUT: api/Contacts/5
        public void Put(int id, [FromBody]Contact value)
        {
            var contact = contacts.SingleOrDefault(t => t.CONTACTSID == id);
            if (contact == null)
            {
                Post(value);
            }
            else
            {
                if (value.Name != null)
                {
                    contact.Name = value.Name;
                }

                if (value.Phones != null)
                {
                    contact.Phones = value.Phones;
                }
            }
        }

        // DELETE: api/Contacts/5
        public HttpResponseMessage Delete(int id)
        {
            if (contacts.SingleOrDefault(t => t.CONTACTSID == id) == null)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            contacts.RemoveAll(t => t.CONTACTSID == id);

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
