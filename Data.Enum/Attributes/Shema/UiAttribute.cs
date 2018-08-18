using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using Newtonsoft.Json.Converters;

namespace X.Data.Utility.Attributes
{

  
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))] 
    public class UiAttribute : Attribute
    {
        /// <summary>
        /// 组件类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Widget Widget { get; set; }

        /// <summary>
        /// 指定 input 的 type 值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        /// <summary>
        /// 加载时是否获得焦点
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Autofocus { get; set; }
        /// <summary>
        /// 文本框中的提示信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Placeholder { get; set; }
        /// <summary>
        /// 自定义类，等同 [ngClass] 值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }
        /// <summary>
        /// 指定宽度，单位：px
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Width { get; set; }
        /// <summary>
        /// 元素组件大小
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Size Size { get; set; }
        /// <summary>
        /// 标签可选帮助
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OptionalHelp { get; set; }
        /// <summary>
        /// 标签可选信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Optional { get; set; }

    }

    public enum Size
    {
        Default,
        large,
        small
    }
    public enum Widget
    {
        autocomplete,
        checkbox,
        radio,
        slider,
        rate,
        select,
        tag,
        text,
        textarea,
        custom,
        date,
        time,
        transfer,
        upload,
        mention
    }
}

