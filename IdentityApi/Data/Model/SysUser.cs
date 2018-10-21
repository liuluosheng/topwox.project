using Data.Attributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Identity.Api.Data
{
    public class SysUser : IdentityUser<Guid>
    {
        [SchemaColumn]
        [Required]
        [Display(Name = "用户名")]
        public override string UserName {
            get ;
            set ;
        }

        [SchemaColumn]
        [Required]
        [Display(Name = "Email")]
        public override string Email {
            get;
            set;
        }

        [SchemaColumn]
        [Required]
        [Display(Name = "联系电话")]
        public override string PhoneNumber {
            get;
            set;
        }

        [SchemaColumn]
        [Required]
        [Display(Name = "部门")]
        public string Department { get; set; }
    }
}
