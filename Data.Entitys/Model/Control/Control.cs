using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace X.Data.Model.Control
{
    [JsonObject(
        NamingStrategyType = typeof(CamelCaseNamingStrategy),

        ItemNullValueHandling = NullValueHandling.Ignore)
     ]
    public class Control
    {
        public string Name { get; set; }
        public string Title { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ControlType Type { get; set; }
        public string Description { get; set; }
        public string PlaceHolder { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Required { get; set; }
    }
    /// <summary>
    /// 文本框
    /// </summary>
    public class Text : Control
    {
        public string Pattern { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }
    /// <summary>
    /// 数值框
    /// </summary>
    public class Number : Control
    {

        public object Minimum { get; set; }
        public object Maximum { get; set; }
    }
    /// <summary>
    /// 自动提示选择
    /// </summary>
    public class Autocomplete : Control
    {
        public string Search { get; set; }
        public string Label { get; set; }
        public string DataType { get; set; }
    }
    /// <summary>
    /// 选择框
    /// </summary>
    public class Select : Control
    {
        public string[] Options { get; set; }
    }
    /// <summary>
    /// 上传组件
    /// </summary>
    public class Upload : Control
    {
        public string Action { get; set; }
        public string ButtonText { get; set; }
        public string FileType { get; set; }
    }

}
