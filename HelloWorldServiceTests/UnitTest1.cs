using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

using FluentAssertions;

namespace HelloWorldServiceTests
{
    public class Contact
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date_added")]
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

    public class CandyContact
    {
        public int Id { get; set; }
        public bool Candy { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        HttpClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/helloworldservice/api/");
        }

        [TestMethod]
        public void AddNewContact()
        {
            var postResult = AddContact();

            var json = postResult.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject<CandyContact>(json);
            
            //Assert.AreEqual(
            //    System.Net.HttpStatusCode.NoContent
            //    , postResult.StatusCode, "Hey this is wrong");

            postResult.StatusCode
                .Should()
                .Be(System.Net.HttpStatusCode.OK, "hey fluent failed");
        }

        private HttpResponseMessage AddContact()
        {
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
        
            return postResult;
        }

        [TestMethod]
        public void DeleteContact()
        {
        }

        [TestMethod]
        public void GetSpecificContact()
        {
            var postResult = AddContact();

            var json = postResult.Content.ReadAsStringAsync().Result;
            var candy = JsonConvert.DeserializeObject<CandyContact>(json);

            var getResult = client.GetAsync("contacts/" + candy.Id).Result;
            var json2 = getResult.Content.ReadAsStringAsync().Result;
            var contact = JsonConvert.DeserializeObject<Contact>(json2);

            contact.Id.Should().Be(candy.Id);
        }
       
    }
}
