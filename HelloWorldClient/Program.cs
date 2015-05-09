using System;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        [JsonProperty("phone_number")]
        public string Number { get; set; }

        [JsonProperty("phone_type")]
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

            client.BaseAddress = new Uri("http://localhost/helloworldservice/api/contacts/");

            client.DefaultRequestHeaders.Add("Api-Key", "value");

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
          
            var postResult = SendAsync(client, HttpMethod.Post, newContact);

            Console.WriteLine(postResult.StatusCode);

            var result = SendAsync(client, HttpMethod.Get);
            var json = result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(json);

            var obj = JsonConvert.DeserializeObject<List<Contact>>(json);

            Console.WriteLine(obj[0].Id);

            // Ex1. Add Delete
            var deleteResult = DeleteContact(client, obj[0]);

            Console.ReadLine();
        }

        public static HttpResponseMessage DeleteContact(HttpClient client, Contact contact)
        {
            return SendAsync(client, HttpMethod.Delete, contact);
        }

        public static HttpResponseMessage SendAsync(HttpClient client, HttpMethod method, Contact contact = null)
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = method,
            };

            if (method == HttpMethod.Post)
            {
                var newJson = JsonConvert.SerializeObject(contact);
                var postContent = new StringContent(newJson, System.Text.Encoding.UTF8, "application/json");
                requestMessage.Content = postContent;
            }
            if (method == HttpMethod.Delete)
            {
                requestMessage.RequestUri = new Uri( client.BaseAddress.ToString() + contact.Id);
            }

            var response = client.SendAsync(requestMessage).Result;

            return response;
        }
    }
}