using Data.Attributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace IdentityServer.Model
{

    public class User : IdentityUser<Guid>
    {
        [SchemaColumn]
        [Required]
        [Display(Name = "用户名")]
        public override string UserName
        {
            get;
            set;
        }

        [SchemaColumn]  
        [Display(Name = "Email")]
        public override string Email
        {
            get;
            set;
        }

        [SchemaColumn]   
        [Display(Name = "联系电话")]
        public override string PhoneNumber
        {
            get;
            set;
        }

        [SchemaColumn] 
        [Display(Name = "部门")]
        public string Department { get; set; }

        [SchemaColumn]
        [NotMapped]
        [DataMember]
        [Description("将重置用户的密码！")]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}
