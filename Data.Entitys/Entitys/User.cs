using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace X.Data.Entitys
{
    public class User : EntityBase
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        [Display(Name = "区域")]
        [Required]
        public string Area { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [Display(Name = "职位")]
        public string Position { get; set; }
    }
}
