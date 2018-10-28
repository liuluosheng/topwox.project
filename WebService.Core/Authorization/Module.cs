using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebService.Core.Authorization
{
   public class Module
    {
        public string Key { get; set; }
        public string Descripton { get; set; }
        public List<OperationDescription> Operations { get; set; } = new List<OperationDescription>();
    }

    public class OperationDescription
    {
       public string Description;

        [JsonConverter(typeof(StringEnumConverter))]
        public Operation Operation;
    }
}
