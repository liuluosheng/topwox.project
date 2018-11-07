using Microsoft.AspNet.OData.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Topwox.Data.Attributes;
using Topwox.Data.Attributes.Shema;
using Topwox.Data.Entitys;

namespace Topwox.Data.Shema.Control
{
    public class ControlFactory
    {
        private IConfiguration _configuration;
        public ControlFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Schema CreateSchema(Type type)
        {
            var create = Create(type);
            return new Schema
            {
                Properties = create.Item1,
                Expand = create.Item2,
                Type = type.Name
            };
        }
        private Tuple<List<ControlBase>, List<string>> Create(Type type)
        {
            var controls = new List<ControlBase>();
            var expands = new List<string>();
            var schemaProps = type.GetProperties().Where(p => p.GetCustomAttribute<DataMemberAttribute>() is DataMemberAttribute);
            if (!schemaProps.Any())
                schemaProps = type.GetProperties().Where(p => p.GetCustomAttribute<SchemaIgnoreAttribute>() == null);
            foreach (var p in schemaProps)
            {
                if (Create(p) is ControlBase control)
                {
                    controls.Add(control);
                    if (control.ColumnSetting?.NavigationExpression != null)
                    {
                        expands.Add(control.ColumnSetting?.NavigationExpression.Split('.').First());
                    }
                    if (control.ColumnSetting != null && p.GetCustomAttributes<NotMappedAttribute>() is NotMappedAttribute)
                    {
                        control.ColumnSetting.Editable = false;
                        control.ColumnSetting.Filterable = false;
                        control.ColumnSetting.Sortable = false;
                    }
                }
            }
            return Tuple.Create(controls, expands);
        }

        private ControlBase Create(PropertyInfo prop)
        {
            ControlBase control = null;
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
                            Maximum = rangeAtt?.Maximum,
                            Minimum = rangeAtt?.Minimum
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
                        control = new ControlBase { Type = controlType };
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
                control = new Autocomplete
                {
                    Type = ControlType.Autocomplete,
                    Name = prop.Name,
                    DataType = autoCompleteAttribute.DataType.ToLower(),
                    Label = autoCompleteAttribute.Label,
                    Search = autoCompleteAttribute.Search
                };
            }
            if (control != null)
            {

                var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                control.Title = displayAttribute?.Name ?? prop.Name;
                control.Description = displayAttribute?.Description ?? null;
                if (control.Description == null)
                {
                    var descriptionAttribute = prop.GetCustomAttribute<DescriptionAttribute>();
                    control.Description = descriptionAttribute?.Description ?? null;
                }
                if (control.Title == null)
                {
                    var displayNameAttribute = prop.GetCustomAttribute<DisplayNameAttribute>();
                    control.Title = displayNameAttribute?.DisplayName ?? null;
                }
                var placeHolderAttribute = prop.GetCustomAttribute<PlaceHolderAttribute>();
                control.PlaceHolder = placeHolderAttribute?.Value ?? null;

                if (prop.GetCustomAttribute<RequiredAttribute>() is RequiredAttribute)
                {
                    control.Required = true;
                }
                if (prop.GetCustomAttribute<NotFilterableAttribute>() is NotFilterableAttribute || prop.GetCustomAttribute<NonFilterableAttribute>() is NonFilterableAttribute)
                {
                    control.ColumnSetting.Filterable = false;
                }
                if (prop.GetCustomAttribute<UnsortableAttribute>() is UnsortableAttribute || prop.GetCustomAttribute<NotSortableAttribute>() is NotSortableAttribute)
                {
                    control.ColumnSetting.Sortable = false;
                }
                if (prop.GetCustomAttribute<NotEditableAttribute>() is NotEditableAttribute || !prop.CanWrite)
                {
                    control.ColumnSetting.Editable = false;
                }
                if (prop.GetCustomAttribute<NavigationExpressionAttribute>() is NavigationExpressionAttribute navigationExpression)
                {
                    control.ColumnSetting.NavigationExpression = navigationExpression.Expression;
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
