using System;
using System.Collections.Generic;
using System.Text;

namespace X.Data.Utility.Attributes
{
    /// <summary>
    /// JSON Schena 设置限定倍数
    /// </summary>
    public class MultipleOfAttribute : Attribute
    {
        public int Value { get; set; }
    }
}
