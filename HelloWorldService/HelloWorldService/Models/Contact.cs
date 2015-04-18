using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HelloWorldService.Models
{
    public class Contact
    {
        [JsonProperty("id")]
        public int CONTACTSID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date_added")]
        public DateTime DateAdded { get; set; }

        [JsonProperty("phones", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Phone[] Phones { get; set; }
    }

    public class Phone
    {
        [JsonProperty("phone_number", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Number { get; set; }

        [JsonProperty("phone_type")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PhoneType PhoneType { get; set; }
    }

    public enum PhoneType
    {
        Nil,
        Home,
        Mobile,
    }
}