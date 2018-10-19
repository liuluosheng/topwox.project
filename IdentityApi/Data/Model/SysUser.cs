using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.IdentityApi.Data
{
    public class SysUser : IdentityUser<Guid>
    {
        public string Department { get; set; }
    }
}
