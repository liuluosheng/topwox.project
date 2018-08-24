using Data.Enums;
using X.Data.Utility.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace X.Data.Dto
{
    public class User
    {
        [Display(Name = "用户名")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Ui(Type = "password")]
        public string PassWord { get; set; }

        [Display(Name = "Email")]
        [Required]
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "Email格式不正确")]
        public string Email { get; set; }

        [Display(Name = "联系电话")]
        public string PhoneNumber { get; set; }

        [Display(Name = "区域")]
        public string Area { get; set; }

        [Display(Name = "年龄")]
        public int Age { get; set; }

        [Display(Name = "注册日期")]
        [Ui(Autofocus = true)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Color")]
        public Color UserColor { get; set; }
    }
}
