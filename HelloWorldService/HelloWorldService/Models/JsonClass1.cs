using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWorldService.Models
{
    /*
{
  "firstName": "John",
  "lastName": "Smith",
  "isAlive": true,
  "age": 25,
  "phoneNumbers": [
    {
      "type": "home",
      "number": "212 555-1234"
    },
    {
      "type": "office",
      "number": "646 555-4567"
    }
  ],
  "children": [],
  "spouse": null
}
     */

    public class JsonClass1PhoneNumbers
    {
        public string type;
        public string number;
    }

    public class JsonClass1Child
    {
    }

    public class JsonClass1
    {
        public string firstName;
        public string lastName;
        public bool isAlive;
        public int age;
        public JsonClass1PhoneNumbers[] phoneNumbers;
        public JsonClass1Child[] children; 
        // OR: public JsonClass1[] children
        public JsonClass1 spouse;
    }
}