using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HelloWorld.Models
{
    /// <summary>
    /// Contact doc
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Id doc
        /// </summary>
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
        [JsonProperty("number", DefaultValueHandling=DefaultValueHandling.Ignore)]
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
}