using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace X.Data.Attributes
{
  
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ODataPropertyAttribute : Attribute
    {
        public ODataPropertyAttribute(string expression)
        {
            Expression = expression;
        }

        /// <summary>
        //指定导航属性的字段，如：Purchasing.Name
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Expression  { get; set; }
    }
}
