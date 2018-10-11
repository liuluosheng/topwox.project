using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace X.Data.Attributes
{

    /// <summary>
    /// 如果实体类中有属性指定了该属性，则schema-table的列则是指定该属性的列，否则为实体类的除标有SchemaIgnore属性的全部列。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SchemaColumnAttribute : Attribute
    {
        /// <summary>
        ///指定要显示为指定的属性值，如：Purchasing.Name
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DisplayExpression  { get; set; }

        /// <summary>
        ///指定列是否可搜索
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool Searchable { get; set; } = true;

        /// <summary>
        ///指定列是否可排序
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool Sortable { get; set; } = true;

        /// <summary>
        ///指定列是否可排序
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool Editable { get; set; } = true;

    }
}
