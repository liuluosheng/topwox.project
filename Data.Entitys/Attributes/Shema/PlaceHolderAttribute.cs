using System;
using System.Collections.Generic;
using System.Text;

namespace Topwox.Data.Attributes.Shema
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PlaceHolderAttribute : Attribute
    {
        public PlaceHolderAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
}
