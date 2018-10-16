using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IdentityServer.Model
{
    public enum SystemRole
    {
        [Description("超级管理员，拥有所有权限")]
        SuperAdmin = 1 << 1,

        [Description("管理员，拥有业务流程所有权限")]
        Admin = 1 << 2
    }
}
