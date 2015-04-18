using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWorldService.Models;

namespace HelloWorldService.Controllers
{
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
        public void Post([FromBody]Contact value)
        {
            if (value == null) return;

            value.CONTACTSID = nextId++;
            contacts.Add(value);
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
        public void Delete(int id)
        {
            contacts.RemoveAll(t => t.CONTACTSID == id);
        }
    }
}
