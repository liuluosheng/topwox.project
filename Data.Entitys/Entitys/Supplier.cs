using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Topwox.Data.Attributes;

namespace Topwox.Data.Entitys
{
    [GeneratedOdataController]
   public class Supplier :EntityBase
    {
        [Display(Name = "公司名称")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "联系电话")]
        [Required]
        public string PhoneNumber { get; set; }
        [Display(Name = "公司地址")]
        [Required]
        public string Address { get; set; }
    }
}
