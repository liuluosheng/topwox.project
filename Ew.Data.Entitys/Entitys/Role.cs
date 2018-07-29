using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace Data.Entitys
{
    public class Role : IdentityRole
    {
        public string Permissions { get; set; }

        
    }
}
