
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Linq;
using X.Data.Attributes;
using Microsoft.Extensions.Configuration;
namespace X.Data.Model
{

    /// <summary>
    /// 构建 ngAlain 适用的Schema
    /// </summary>
    public class JsonSchema
    {
        public JsonSchema(Type type, IConfiguration configuration)
        {
            Properties = new Dictionary<string, object>();
            var prop = type.GetProperties();
            foreach (var p in prop)
            {
                if (p.GetCustomAttribute<SchemaIgnoreAttribute>() != null) continue;
                var datatypeAtt = p.GetCustomAttribute<DataTypeAttribute>();
                var value = new Dictionary<string, object> { { "type", SchemaType(p) } };
                value.Add("name", p.Name);
                //标题与描述
                var displayAtt = p.GetCustomAttribute<DisplayAttribute>();
                if (displayAtt != null)
                {
                    if (!string.IsNullOrEmpty(displayAtt.Name)) value.Add("title", displayAtt.Name);
                    if (!string.IsNullOrEmpty(displayAtt.Description)) value.Add("description", displayAtt.Description);
                }

                if (value.All(k => k.Key != "description"))
                {
                    var descriptionAtt = p.GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAtt != null && !string.IsNullOrEmpty(descriptionAtt.Description))
                    {
                        value.Add("description", descriptionAtt.Description);
                    }
                }

                if (value.All(k => k.Key != "title"))
                {
                    var desplayNameAtt = p.GetCustomAttribute<DisplayNameAttribute>();
                    if (desplayNameAtt != null && !string.IsNullOrEmpty(desplayNameAtt.DisplayName))
                    {
                        value.Add("title", desplayNameAtt.DisplayName);
                    }
                    else
                    {
                        value.Add("title", p.Name);
                    }
                }
                //最小值，最大值
                if (p.GetCustomAttribute<RangeAttribute>() is RangeAttribute rangeAtt)
                {
                    value.Add("minimum", rangeAtt.Minimum);
                    value.Add("maximum", rangeAtt.Maximum);
                }

                //最小长度      
                if (p.GetCustomAttribute<MinLengthAttribute>() is MinLengthAttribute minLengthAtt)
                {
                    value.Add("minLength", minLengthAtt.Length);
                }

                //最大长度
                var maxLengthAtt = p.GetCustomAttribute<MaxLengthAttribute>();
                if (maxLengthAtt != null)
                {
                    value.Add("maxLength", maxLengthAtt.Length);
                }
                //正则
                var regExpressAtt = p.GetCustomAttribute<RegularExpressionAttribute>();
                if (regExpressAtt != null)
                {
                    value.Add("pattern", regExpressAtt.Pattern);
                }
     
                //必需项
                var requiredAtt = p.GetCustomAttribute<RequiredAttribute>();
                if (requiredAtt != null)
                {
                    value.Add("required", true);
                }


                if (p.GetCustomAttribute<UploadAttribute>() is UploadAttribute upload)
                {
                    upload.Action = upload.Action ?? configuration["AppSettings:FileUploadUrl"];
                    value.Add("upload", upload);
                }

                //枚举
                if (p.PropertyType.IsEnum)
                {
                    value.Add("enum", Enum.GetNames(p.PropertyType));
                }

                Properties.Add(p.Name, value);
            }
        }
        [JsonProperty(PropertyName = "properties", Order = 3)]
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// 判断是否为数值类型。
        /// </summary>
        /// <param name="t">要判断的类型</param>
        /// <returns>是否为数值类型</returns>
        private bool IsNumericType(Type t)
        {
            var tc = Type.GetTypeCode(t);
            return (
                t.IsPrimitive &&
                t.IsValueType &&
                !t.IsEnum &&
                tc != TypeCode.Char &&
                tc != TypeCode.Boolean) ||
                tc == TypeCode.Decimal ||
                t == typeof(int?) ||
                t == typeof(decimal?) ||
                t == typeof(double?);

        }

        private string SchemaType(PropertyInfo p)
        {
          
            Type t = p.PropertyType;
            if (t.IsGenericType)
            {
                t = t.GenericTypeArguments.FirstOrDefault();
            }
                if (t.IsEnum)
            {
                return "enum";
            }

            if (p.GetCustomAttribute<UploadAttribute>() != null)
            {
                return "upload";
            }

            if (IsNumericType(t))
            {
                return "number";
            }
            if (
                t == typeof(DateTime) ||
                t == typeof(DateTime?) ||
                t == typeof(DateTimeOffset) ||
                t == typeof(DateTimeOffset?))
            {
                return "datetime";
            }
            if (
                t == typeof(string) ||
                t == typeof(Guid?) ||
                t == typeof(Guid))
            {
                return "string";
            }
            return t.Name.ToLower();
        }
    }

}
