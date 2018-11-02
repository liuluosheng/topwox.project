using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EnumsNET;
namespace WebService.Core.Authorization
{

    /// <summary>
    ///定义模块中的所特有的操作项
    ///名称约定：模块名称_操作名称
    /// </summary>
    [Flags]
    public enum PrivateOperation
    {
        [Description("系统菜单项管理")] SystemMenu_DataAdmin = 1000,
    }
    /// <summary>
    /// 定义所有模块都会具有的操作
    /// </summary>
    [Flags]
    public enum PublicOperation
    {
        [Description("查询")] Read = 2000,
        [Description("新建")] Create = Read | 2001,
        [Description("更新")] Update = Read | 2002,
        [Description("删除")] Delete = Read | 2003,
        [Description("编辑")] Edit = Create | Update | Delete,
    }
}
