using Topwox.Data.Attributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Topwox.IdentityServer.Model
{

    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
