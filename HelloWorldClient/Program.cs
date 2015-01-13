using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            
            var result = client.GetAsync("contacts").Result;
            
            var json = result.Content.ReadAsStringAsync().Result;
            
            Console.WriteLine(json);

            var obj = JsonConvert.DeserializeObject<List<Contact>>(json);

            foreach (var entry in obj)
            {
                Console.WriteLine(entry.Id);
            }
            
            Console.ReadLine();
        }
    }
}