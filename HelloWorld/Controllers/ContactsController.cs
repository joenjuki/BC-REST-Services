using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWorld.Models;
using System.Net.Http.Formatting;

namespace HelloWorld.Controllers
{
    public class ContactsController : ApiController
    {
        private static int nextId = 100;

        public static List<Contact> contacts = new List<Contact>();

        // GET: api/Contacts
        public ContactResponse Get()
        {
            //int x = 1;
            //x = x / (x - 1);
            //return contacts;
            return new ContactResponse
            {
                Header = new StandardResponseHeader { Status = "success", StatusList = null },

                ResponseBody = new ContactResponseBody
                {
                    Items = contacts.ToArray()
                }
            };
        }

        // GET: api/Contacts/5
        public Contact Get(int id)
        {
            return contacts.SingleOrDefault(t => t.Id == id);
        }

        // POST: api/Contacts
        public void Post([FromBody]Contact contact)
        {
            if (contact != null)
            {
                contact.Id = nextId++;
                contacts.Add(contact);
            }
        }

        // PUT: api/Contacts/5
        public void Put(int id, [FromBody]Contact value)
        {
        }

        // DELETE: api/Contacts/5
        public void Delete(int id)
        {
            contacts.RemoveAll(t => t.Id == id);
        }
    }
}
