using HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWorld
{
    public enum StatusType
    {
        Nil,
        [System.Runtime.Serialization.EnumMember(Value = "informational")]
        Informational,
        [System.Runtime.Serialization.EnumMember(Value = "warning")]
        Warning,
        [System.Runtime.Serialization.EnumMember(Value = "error")]
        Error,
    }

    public class Status
    {
        [Newtonsoft.Json.JsonProperty("type")]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public StatusType Type { get; set; }
        [Newtonsoft.Json.JsonProperty("code", DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public string Code { get; set; }
        [Newtonsoft.Json.JsonProperty("message")]
        public string Message { get; set; }
        public Status(StatusType type, string code, string message, params object[] args)
        {
            Type = type;
            Code = code;
            Message = message;
            if (args != null && args.Length > 0)
            {
                Message = string.Format(message, args);
            }
        }
    }
    public class StatusList : List<Status>
    {
        public bool HasErrors
        {
            get { return Exists(t => t.Type == StatusType.Error); }
        }
        public bool HasWarnings
        {
            get { return Exists(t => t.Type == StatusType.Warning); }
        }
        public bool HasStatus(params Status[] args)
        {
            if (args == null || args.Length <= 0)
                return false;
            return Exists(status => System.Array.Find(args, s => string.Compare(status.Code, s.Code, System.StringComparison.InvariantCultureIgnoreCase) == 0) != null);
        }
        public Status AddStatus(Status status, params object[] args)
        {
            string message = (args == null || args.Length <= 0) ? status.Message : string.Format(status.Message, args);
            var innerStatus = new Status(status.Type, status.Code, message);
            Add(innerStatus);
            return innerStatus;
        }
    }

    public class StandardResponseHeader
    {
        [Newtonsoft.Json.JsonProperty("status", Order = 0)]
        public string Status { get; set; }

        [Newtonsoft.Json.JsonProperty("status-list", Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public StatusList StatusList = new StatusList();
       
        [Newtonsoft.Json.JsonProperty("coordination-id", Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
      
        public string CoordinationId { get; set; }
       
        public StandardResponseHeader() { }
        public StandardResponseHeader(string status)
        {
            Status = status;
        }
        public StandardResponseHeader(string status, Status statusItem)
        {
            Status = status;
            StatusList.Add(statusItem);
        }
    }

    public abstract class StandardResponse
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "response-header")]
        public StandardResponseHeader Header = new StandardResponseHeader();
    }

    public class ContactResponse : StandardResponse
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "contact-response-body")]
        public ContactResponseBody ResponseBody { get; set; }
    }

    public class ContactResponseBody
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "items")]
        public Contact[] Items { get; set; }
    }
}