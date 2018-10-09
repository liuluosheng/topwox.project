using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
namespace X.Data.Model
{
    [JsonObject(
     NamingStrategyType = typeof(CamelCaseNamingStrategy),
     ItemNullValueHandling = NullValueHandling.Ignore)
  ]
    public class Schema
    {
        public List<Control.Control> Properties { get; set; }
        public string Type { get; set; }
        public List<string> Expand { get; set; }
    }
}
