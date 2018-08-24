using X.Data.Utility.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace X.Data.Utility
{

    /// <summary>
    /// 构建 ngAlain 适用的Schema
    /// </summary>
    public class JsonSchema
    {
        public JsonSchema(Type type)
        {
            Properties = new Dictionary<string, object>();
            var prop = type.GetProperties();
            foreach (var p in prop)
            {
                var value = new Dictionary<string, object> { { "type", SchemaType(p.PropertyType) } };
                //标题与描述
                var displayAtt = p.GetCustomAttribute<DisplayAttribute>();
                if (displayAtt != null)
                {
                    if (!string.IsNullOrEmpty(displayAtt.Name)) value.Add("title", displayAtt.Name);
                    if (!string.IsNullOrEmpty(displayAtt.Description)) value.Add("description", displayAtt.Description);
                }
                else
                {
                    var descriptionAtt = p.GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAtt != null && !string.IsNullOrEmpty(descriptionAtt.Description))
                    {
                        value.Add("description", descriptionAtt.Description);
                    }
                    var desplayNameAtt = p.GetCustomAttribute<DisplayNameAttribute>();
                    if (descriptionAtt != null && !string.IsNullOrEmpty(desplayNameAtt.DisplayName))
                    {
                        value.Add("title", desplayNameAtt.DisplayName);
                    }
                    else
                    {
                        value.Add("title", p.Name);
                    }
                }
                //默认值
                var defaultAtt = p.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultAtt != null && defaultAtt.Value != null)
                {
                    value.Add("default", defaultAtt.Value);
                }

                //最小值，最大值
                var rangeAtt = p.GetCustomAttribute<RangeAttribute>();
                if (rangeAtt != null)
                {
                    value.Add("minimum", rangeAtt.Minimum);
                    value.Add("maximum", rangeAtt.Maximum);
                }

                //最小长度
                var minLengthAtt = p.GetCustomAttribute<MinLengthAttribute>();
                if (minLengthAtt != null)
                {
                    value.Add("minLength", minLengthAtt.Length);
                }

                //最大长度
                var maxLengthAtt = p.GetCustomAttribute<MaxLengthAttribute>();
                if (minLengthAtt != null)
                {
                    value.Add("maxLength", maxLengthAtt.Length);
                }
                //正则
                var regExpressAtt = p.GetCustomAttribute<RegularExpressionAttribute>();
                if (regExpressAtt != null)
                {
                    value.Add("pattern", regExpressAtt.Pattern);
                }
                //限定倍数
                var multipleOfAtt = p.GetCustomAttribute<MultipleOfAttribute>();
                if (multipleOfAtt != null)
                {
                    value.Add("multipleOf", multipleOfAtt.Value);
                }
                //必需项
                var requiredAtt = p.GetCustomAttribute<RequiredAttribute>();
                if (requiredAtt != null)
                {
                    if (Required == null) Required = new List<string>();
                    Required.Add(p.Name);
                }
                var datatypeAtt = p.GetCustomAttribute<DataTypeAttribute>();
                bool isDate = p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?);
                if (datatypeAtt != null || isDate)
                {
                    value.Add("format", datatypeAtt?.DataType == DataType.DateTime || isDate ? "date-time" : datatypeAtt?.DataType.ToString().ToLower());
                }
                //枚举
                if (p.PropertyType.IsEnum)
                {
                    value.Add("enum", Enum.GetNames(p.PropertyType));
                }
                var uiAtt = p.GetCustomAttribute<UiAttribute>();
                if (uiAtt != null)
                {
                    value.Add("ui", uiAtt);
                }
                Properties.Add(p.Name, value);
            }
        }
        [JsonProperty(PropertyName = "properties", Order = 3)]
        public Dictionary<string, object> Properties { get; set; }


        [JsonProperty(PropertyName = "required", Order = 10, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Required { get; set; }

        /// <summary>
        /// 判断是否为数值类型。
        /// </summary>
        /// <param name="t">要判断的类型</param>
        /// <returns>是否为数值类型</returns>
        private bool IsNumericType(Type t)
        {
            var tc = Type.GetTypeCode(t);
            return (t.IsPrimitive && t.IsValueType && !t.IsEnum && tc != TypeCode.Char && tc != TypeCode.Boolean) || tc == TypeCode.Decimal;
        }

        private string SchemaType(Type t)
        {
            if (IsNumericType(t))
            {
                return "number";
            }
            if (t.IsEnum || t == typeof(string) || t == typeof(DateTime) || t == typeof(DateTime?))
            {
                return "string";
            }
            return t.Name.ToLower();
        }
    }

}
