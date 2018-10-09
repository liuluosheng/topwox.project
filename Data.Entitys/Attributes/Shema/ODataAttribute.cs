using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace X.Data.Attributes
{
  
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class OdataAttribute : Attribute
    {

        /// <summary>
        ///指定属性被Odata $expand，并且指定$select 的列
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExpandNames { get; set; }
        /// <summary>
        /// 指定属性被Odata $expand 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Expand { get; set; } = true;
   

    }
}
