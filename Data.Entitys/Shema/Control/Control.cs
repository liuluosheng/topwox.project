using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Topwox.Data.Attributes;

namespace Topwox.Data.Shema.Control
{
    [JsonObject(
        NamingStrategyType = typeof(CamelCaseNamingStrategy),
        ItemNullValueHandling = NullValueHandling.Ignore)
     ]
    public class ControlBase
    {
        public string Name { get; set; }
        public string Title { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ControlType Type { get; set; }
        public string Description { get; set; }
        public string PlaceHolder { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Required { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ColumnSettings ColumnSetting { get; set; } = new ColumnSettings();

        public class ColumnSettings
        {
            public bool Sortable { get; set; } = true;
            public bool Editable { get; set; } = true;
            public bool Filterable { get; set; } = true;
            public string NavigationExpression { get; set; }
        }
    }

    /// <summary>
    /// 文本框
    /// </summary>
    public class Text : ControlBase
    {
        public string Pattern { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }
    /// <summary>
    /// 数值框
    /// </summary>
    public class Number : ControlBase
    {

        public object Minimum { get; set; }
        public object Maximum { get; set; }
    }
    /// <summary>
    /// 自动提示选择
    /// </summary>
    public class Autocomplete : ControlBase
    {
        public string Search { get; set; }
        public string Label { get; set; }
        public string DataType { get; set; }
    }
    /// <summary>
    /// 选择框
    /// </summary>
    public class Select : ControlBase
    {
        public string[] Options { get; set; }
    }
    /// <summary>
    /// 上传组件
    /// </summary>
    public class Upload : ControlBase
    {
        public string Action { get; set; }
        public string ButtonText { get; set; }
        public string FileType { get; set; }
    }

}
