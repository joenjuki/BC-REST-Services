using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using WebActivatorEx;
using Newtonsoft.Json;
using Swashbuckle.Swagger;

[assembly: PreApplicationStartMethod(typeof(HelloWorld.SwaggerConfig), "Register")]

namespace HelloWorld
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            Swashbuckle.Bootstrapper.Init(GlobalConfiguration.Configuration);

            // NOTE: If you want to customize the generated swagger or UI, use SwaggerSpecConfig and/or SwaggerUiConfig here ...
            Swashbuckle.Application.SwaggerSpecConfig.Customize(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
                c.ModelFilter<JsonModelFilter>();
            });
        }
        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\HelloWorldService.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }

    public class JsonModelFilter : IModelFilter
    {
        public void Apply(DataType model, DataTypeRegistry dataTypeRegistry, Type type)
        {
            var jsonObjectAttribute = GetJsonObjectAttribute(type);
            if (jsonObjectAttribute != null)
            {
                model.Id = jsonObjectAttribute.Id;
            }
            foreach (var keyValuePair in model.Properties)
            {
                if (keyValuePair.Value.Items != null)
                {
                    var dotNetTypeName = keyValuePair.Value.Items.Ref;
                    if (!string.IsNullOrEmpty(dotNetTypeName))
                    {
                        var refType = FindType(dotNetTypeName);
                        var refTypeJsonAttribute = GetJsonObjectAttribute(refType);
                        if (refTypeJsonAttribute != null && !string.IsNullOrEmpty(refTypeJsonAttribute.Id))
                        {
                            keyValuePair.Value.Items.Ref = refTypeJsonAttribute.Id;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(keyValuePair.Value.Ref))
                {
                    var dotNetTypeName = keyValuePair.Value.Ref;
                    var refType = FindType(dotNetTypeName);
                    var refTypeJsonAttribute = GetJsonObjectAttribute(refType);
                    if (refTypeJsonAttribute != null && !string.IsNullOrEmpty(refTypeJsonAttribute.Id))
                    {
                        keyValuePair.Value.Ref = refTypeJsonAttribute.Id;
                    }
                }
            }
            foreach (var property in type.GetProperties())
            {
                var jsonPropertyAttribute =
                    Attribute.GetCustomAttribute(property, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
                if (jsonPropertyAttribute != null)
                {
                    var swashbuckleProp = model.Properties[property.Name];
                    model.Properties.Remove(property.Name);
                    model.Properties.Add(jsonPropertyAttribute.PropertyName, swashbuckleProp);
                }
            }
        }
        private static Type FindType(string dotNetTypeName)
        {
            return Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == dotNetTypeName);
        }
        private static JsonObjectAttribute GetJsonObjectAttribute(Type type)
        {
            var jsonObjectAttribute =
                Attribute.GetCustomAttribute(type, typeof(JsonObjectAttribute)) as JsonObjectAttribute;
            return jsonObjectAttribute;
        }
    }
}