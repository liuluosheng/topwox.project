using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using X.Data.Attributes;
using X.Data.Attributes.Shema;
using X.Data.Entitys;

namespace X.Data.Model.Control
{
    public class ControlFactory
    {
        private IConfiguration _configuration;
        public ControlFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Control> Create(Type type)
        {
            var controls = new List<Control>();
            foreach (var p in type.GetProperties())
            {
                if (p.GetCustomAttribute<SchemaIgnoreAttribute>() == null)
                {
                    controls.Add(Create(p));
                }
            }
            return controls;
        }
        public Control Create(PropertyInfo prop)
        {
            Control control = null;
            if (GetPropType(prop) is ControlType controlType)
            {
                switch (controlType)
                {
                    case ControlType.Number:
                        //最小值，最大值
                        var rangeAtt = prop.GetCustomAttribute<RangeAttribute>();
                        control = new Number
                        {
                            Type = controlType,
                            Maximum = (int)rangeAtt?.Maximum,
                            Minimum = (int)rangeAtt?.Minimum
                        };
                        break;
                    case ControlType.Text:
                        var minLengthAtt = prop.GetCustomAttribute<MinLengthAttribute>();
                        var maxLengthAttribute = prop.GetCustomAttribute<MaxLengthAttribute>();
                        var regularExpressionAttribute = prop.GetCustomAttribute<RegularExpressionAttribute>();
                        control = new Text
                        {
                            Type = controlType,
                            MinLength = minLengthAtt?.Length,
                            MaxLength = maxLengthAttribute?.Length,
                            Pattern = regularExpressionAttribute?.Pattern
                        };
                        break;
                    case ControlType.Select:
                        control = new Select
                        {
                            Type = controlType,
                            Options = Enum.GetNames(prop.PropertyType)
                        };
                        break;
                    case ControlType.Switch:
                    case ControlType.DateTime:
                        control = new Control { Type = controlType };
                        break;
                }
            }
            if (prop.GetCustomAttribute<UploadAttribute>() is UploadAttribute uploadAttribute)
            {
                control = new Upload
                {
                    Action = uploadAttribute.Action ?? _configuration["AppSettings:FileUploadUrl"],
                    ButtonText = uploadAttribute.ButtonText,
                    FileType = uploadAttribute.FileType,
                    Type = ControlType.Upload
                };
            }
            if (prop.GetCustomAttribute<AutoCompleteAttribute>() is AutoCompleteAttribute autoCompleteAttribute)
            {
                var foreignKey = prop.GetCustomAttribute<ForeignKeyAttribute>();
                control = new Autocomplete
                {
                    Type = ControlType.Autocomplete,
                    Name = foreignKey?.Name,
                    DataType = prop.PropertyType.Name.ToLower(),
                    Label = autoCompleteAttribute.Label,
                    Search = autoCompleteAttribute.Search
                };
            }
            if (control != null)
            {

                var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                control.Title = displayAttribute?.Name ?? null;
                control.Description = displayAttribute?.Description ?? null;

                var descriptionAttribute = prop.GetCustomAttribute<DescriptionAttribute>();
                control.Description = descriptionAttribute?.Description ?? null;

                var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                control.Description = displayNameAttribute?.DisplayName ?? null;

                var placeHolderAttribute = prop.GetCustomAttribute<PlaceHolderAttribute>();
                control.Description = placeHolderAttribute?.Value ?? null;

                if (prop.GetCustomAttribute<RequiredAttribute>() is RequiredAttribute)
                {
                    control.Required = true;
                }
                control.Name = prop.Name;
            }
            return control;
        }

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
        private ControlType? GetPropType(PropertyInfo p)
        {

            Type t = p.PropertyType;
            if (t.IsGenericType)
            {
                t = t.GenericTypeArguments.FirstOrDefault();
            }
            if (t == typeof(Boolean))
            {
                return ControlType.Switch;
            }
            if (t.IsEnum)
            {
                return ControlType.Select;
            }
            if (IsNumericType(t))
            {
                return ControlType.Number;
            }
            if (
                t == typeof(DateTime) ||
                t == typeof(DateTime?) ||
                t == typeof(DateTimeOffset) ||
                t == typeof(DateTimeOffset?))
            {
                return ControlType.DateTime;
            }
            if (
                t == typeof(string) ||
                t == typeof(Guid?) ||
                t == typeof(Guid))
            {
                return ControlType.Text;
            }
            return null;
        }
    }
}
