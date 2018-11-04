﻿using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Attributes;
using Data.Attributes.Shema;
using Data.Entitys;

namespace Data.Entitys
{
    [GeneratedOdataController]
    [Description("员工信息")]
    public class Employees : EntityBase
    {

        [Display(Name = "姓名")]
        [Required]
        [MaxLength(10), MinLength(5)]
        public string Name { get; set; }

        [Display(Name = "联系电话")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "区域")]
        [Required]
        [Upload]
        public string Area { get; set; }
        [Display(Name = "职位")]
        [Required]
        public string Position { get; set; }

        [Display(Name = "Email")]
        [Required]
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "Email格式不正确")]
        public string Email { get; set; }

        [Display(Name = "年龄", Description = "请输入10到60的范围")]
        [Range(10, 60)]
        public int Age { get; set; }

        [Display(Name = "性别")]
        public Gender Gender { get; set; }

        [Display(Name = "是否党员")]
        public bool? IsPartyMember { get; set; }

        [Display(Name = "入职日期")]
        public DateTimeOffset? EntryDate { get; set; }

        [Display(Name = "系统ID"), SchemaIgnore]
        public Guid? IdentityUserId { get; set; }

    }
}
