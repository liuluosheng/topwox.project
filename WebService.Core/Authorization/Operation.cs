using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EnumsNET;
namespace Topwox.WebService.Core.Authorization
{

    /// <summary>
    ///定义模块中的所特有的操作项
    ///名称约定：模块名称_操作名称
    /// </summary>
    [Flags]
    public enum Operation : long
    {

        [Public, Description("查询")] Read = 1 << 1,
        [Public, Description("新建")] Create = 1 << 2,
        [Public, Description("更新")] Update = 1 << 3,
        [Public, Description("删除")] Delete = 1 << 4,
        [Public, Description("模块管理员")] Edit = Create | Read | Update | Delete,

        [Description("系统菜单项 管理")] SystemMenu_DataAdmin
    }


    /// <summary>
    /// 定义枚举操作是一个公共模块的操作
    /// </summary>
    public class PublicAttribute : Attribute
    {
    }
}
