using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.IdentityApi.Data
{
    public class SysRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
