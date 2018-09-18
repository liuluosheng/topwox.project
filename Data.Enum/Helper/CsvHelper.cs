using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Reflection;

namespace X.Utility
{
    /// <summary>
    ///普通aspx页面用法
    /// Response.AppendHeader("content-disposition", "attachment;filename=1.csv");
    ///Response.ContentType = "text/csv";
    ///Response.ContentEncoding = Encoding.UTF8;
    ///CsvHelper.GenerateData(dt, Response.OutputStream);
    ///mvc用法
    ///return File(CsvHelper.GenerateData(dt), "text/csv", "1.csv");
    /// </summary>
    public sealed class CsvHelper
    {
        private CsvHelper() { }

        /// <summary>
        /// 将指定dt转换成二进制数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static byte[] GenerateData(DataTable dt)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                GenerateData(dt, ms);
                ms.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将指定列表转换成二进制数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static byte[] GenerateData<T>(IEnumerable<T> list)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                GenerateData<T>(list, ms);
                ms.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 使用指定流输出二进制数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="outputStream"></param>
        public static void GenerateData<T>(IEnumerable<T> list, Stream outputStream)
        {
            Type t = typeof(T);
            StreamWriter sw = new StreamWriter(outputStream, System.Text.Encoding.UTF8);

            object o = Activator.CreateInstance(t);
            var type = o.GetType();

            PropertyInfo[] props = o.GetType().GetProperties();

            //將資料表的欄位名稱寫出
            foreach (PropertyInfo pi in props)
            {
                string name;
                if (IsNeedToDisplay(pi, out name))
                {
                    sw.Write(name + ",");
                }
            }
            sw.WriteLine();

            //將資料表的資料寫出
            foreach (T item in list)
            {
                foreach (PropertyInfo pi in props)
                {
                    string name;
                    if (IsNeedToDisplay(pi, out name))
                    {
                        string whatToWrite =
                         Convert.ToString(ReplaceNoNeedChars(item.GetType().GetProperty(pi.Name).GetValue(item, null)) + ',');
                        sw.Write(whatToWrite);
                    }
                }
                sw.WriteLine();
            }

            sw.Close();
        }

        /// <summary>
        /// 判断List中实体类的某个字段是否需要显示
        /// </summary>
        /// <param name="isExplicit"></param>
        /// <param name="info"></param>
        private static bool IsNeedToDisplay(PropertyInfo info, out string name)
        {
            var attrs = info.GetCustomAttributes(typeof(CsvFieldAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                var temp = attrs[0] as CsvFieldAttribute;
                if (!temp.IsDisplay)
                {
                    name = "";
                    return false;
                }
            }
            var names = info.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (names != null && names.Length > 0)
            {
                var a = names[0] as DisplayAttribute;
                name = a.Name;
            }
            else
            {
                name = info.Name;
            }
            return true;
        }

        /// <summary>
        /// 使用指定流输出二进制数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="outputStream"></param>
        public static void GenerateData(DataTable dt, Stream outputStream)
        {
            StreamWriter sw = new StreamWriter(outputStream, System.Text.Encoding.UTF8);
            foreach (DataColumn column in dt.Columns)
            {
                sw.Write(column.ColumnName.ToUpper() + ",");
            }
            sw.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (object o in dr.ItemArray)
                {

                    sw.Write(ReplaceNoNeedChars(o) + ",");
                }
                sw.WriteLine();
            }

            sw.Close();
        }

        private static string ReplaceNoNeedChars(object o)
        {
            if (o == null || Convert.IsDBNull(o))
                return string.Empty;
            else
            {
                string text = o.ToString();
                return System.Uri.UnescapeDataString(System.Uri.EscapeDataString(text.Replace(',', '，')).Replace(@"%0D", "").Replace(@"%0A", ""));
            }
        }
    }

    /// <summary>
    /// 指示字段的显示属性
    /// </summary>
    public sealed class CsvFieldAttribute : Attribute
    {
        public CsvFieldAttribute()
        {
            IsDisplay = true;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplay { get; set; }
    }
}