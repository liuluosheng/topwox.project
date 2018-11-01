using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EnumsNET;
namespace WebService.Core.Authorization
{

    /// <summary>
    ///定义所有模块中的所有操作项
    ///名称约定：模块名称_操作名称
    ///模块名称的Description 请定义在模块对应实体类的[Description]属性中
    /// </summary>
    [Flags]
    public enum Operation
    {
        [Description("查询")] SystemMenu_Read = 1,
        [Description("新建")] SystemMenu_Create = SystemMenu_Read | 2,
        [Description("更新")] SystemMenu_Update = SystemMenu_Read | 3,
        [Description("删除")] SystemMenu_Delete = SystemMenu_Read | 4,
        [Description("编辑")] SystemMenu_Edit = SystemMenu_Create | SystemMenu_Update | SystemMenu_Delete,
    }

    [Flags]
    public enum BaseOperation
    {
        [Description("查询")] Read = 10000,
    }
}
