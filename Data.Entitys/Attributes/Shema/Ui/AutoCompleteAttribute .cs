using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using Newtonsoft.Json.Converters;

namespace X.Data.Attributes
{

    /// <summary>
    /// Autocoplete设定
    /// 与ForeignKey 属性配合使用
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class AutoCompleteAttribute : Attribute
    {
        public AutoCompleteAttribute()
        {
           
        }
        /// <summary>
        /// 按指定的属性来搜索,多项以逗号分隔
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Search { get; set; }

        /// <summary>
        /// 选择后显示的属性
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
        /// <summary>
        /// PlaceHolder
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlaceHolder { get; set; }

    }

}

