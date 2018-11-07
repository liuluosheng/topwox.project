using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
namespace Topwox.Data.Shema
{
    [JsonObject(
     NamingStrategyType = typeof(CamelCaseNamingStrategy),
     ItemNullValueHandling = NullValueHandling.Ignore)
  ]
    public class Schema
    {
        public List<Control.ControlBase> Properties { get; set; }
        public string Type { get; set; }
        public List<string> Expand { get; set; }
    }
}
