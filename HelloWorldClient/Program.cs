using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldClient
{
    public class Contact
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date-added")]
        public DateTime DateAdded { get; set; }

        [JsonProperty("phones")]
        public Phone[] Phones { get; set; }
    }

    public class Phone
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("phone-type")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PhoneType PhoneType { get; set; }
    }

    public enum PhoneType
    {
        Home,
        Mobile,
    }

    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost.fiddler:49270/api/");

            var newContact = new Contact
            {
                Name = "New Name",
                Phones = new[] { 
                    new Phone { 
                        Number = "425-111-2222",
                        PhoneType = PhoneType.Mobile
                    }
                }
            };

            var newJson = JsonConvert.SerializeObject(newContact);

            var postContent = new StringContent(newJson, Encoding.UTF8, "application/json");
            
            var postResult = client.PostAsync("contacts", postContent).Result;

            SendAsync(client, HttpMethod.Post, "contacts", newContact);
           
            Console.WriteLine(postResult.StatusCode);

            // Delete a contact
            var deleteResult = client.DeleteAsync("contacts/101").Result;
            Console.WriteLine(deleteResult.StatusCode);
           
            var result = client.GetAsync("contacts").Result;
            var json = result.Content.ReadAsStringAsync().Result;
            
            Console.WriteLine(json);

            var contacts = JsonConvert.DeserializeObject<List<Contact>>(json);

            foreach (var contact in contacts)
            {
                Console.WriteLine(contact.Id);
            }

            var resultSendAsync = SendAsync(client, HttpMethod.Get, "contacts/101", null);
           
            Console.ReadLine();
        }

        public static HttpResponseMessage SendAsync(HttpClient client, HttpMethod method, string uri, object content)
        {
            HttpRequestMessage request = new HttpRequestMessage();

            request.Method = method;
            request.RequestUri = new Uri("http://localhost.fiddler:49270/api/" + uri);

            if (content != null)
            {
                var jsonString = JsonConvert.SerializeObject(content);

                request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            return client.SendAsync(request).Result;
        }
    }
}