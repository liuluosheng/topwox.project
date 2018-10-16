using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Attributes
{
    /// <summary>
    /// 添加到实体属性上，标识排除该属性生成JSON Schema
    /// 在前端动态表单中将不会显示标识的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
   public class SchemaIgnoreAttribute:Attribute
    {
    }
}
