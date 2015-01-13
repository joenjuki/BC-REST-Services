using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    public class ContactsController : ApiController
    {
        private static int nextId = 100;

        public static List<Contact> contacts = new List<Contact>();
        
        // GET: api/Contacts
        public IEnumerable<Contact> Get()
        {
            return contacts;
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
