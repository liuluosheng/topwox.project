using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EnumsNET;
namespace WebService.Core.Authorization
{
    //定义模块中的所有操作项
    [Flags]
    public enum Operation
    {
        [Description("查询数据")]
        Read = 1 << 1,

        [Description("新建")]
        Create = 1 << 2,

        [Description("更新")]
        Update = 1 << 3,

        [Description("删除")]
        Delete = 1 << 4,

        [Description("上传")]
        Upload = 1 << 5,

        [Description("下载")]
        Export = 1 << 6,

        [Description("数据管理")]
        DataAdmin = Read | Update | Create | Delete | Upload | Export
    }
}
