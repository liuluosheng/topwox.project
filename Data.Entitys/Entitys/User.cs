using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace Data.Entitys
{
    public class User : EntityBase
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string position { get; set; }
    }
}
