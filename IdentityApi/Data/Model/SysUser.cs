using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Identity.Api.Data
{
    public class SysUser : IdentityUser<Guid>
    {
        public string Department { get; set; }
    }
}
