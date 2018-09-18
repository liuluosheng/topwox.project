using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace X.Utility
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ToDynamic<TSource>(this TSource source, string fields = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var dataShapedObject = new ExpandoObject();
            if (string.IsNullOrWhiteSpace(fields))
            {
                // 所有的 public properties 应该包含在ExpandoObject里 
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                foreach (var propertyInfo in propertyInfos)
                {
                    // 取得源对象上该property的值
                    var propertyValue = propertyInfo.GetValue(source);
                    // 为ExpandoObject添加field
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }
                return dataShapedObject;
            }

            // field是使用 "," 分割的, 这里是进行分割动作.
            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                // 使用反射来获取源对象上的property
                // 需要包括public和实例属性， 并忽略大小写.
                var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new Exception($"没有在‘{typeof(TSource)}’上找到‘{propertyName}’这个Property");
                }

                // 取得源对象property的值
                var propertyValue = propertyInfo.GetValue(source);
                // 为ExpandoObject添加field
                ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;
        }
    }
}