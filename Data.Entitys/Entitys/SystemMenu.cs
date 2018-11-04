using Topwox.Data.Attributes;
using Topwox.Data.Attributes.Shema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Topwox.Data.Entitys
{
    [GeneratedOdataController]
    [DisplayName("系统菜单")]
    public class SystemMenu : EntityBase
    {
        [Display(Name = "菜单名称")]
        [Required]
        [SchemaColumn]
        public string Name { get; set; }

        [Display(Name = "顶级菜单")]
        [Required]
        [SchemaColumn]
        public bool Root { get; set; } = false;

        [Display(Name = "路由")]
        [SchemaColumn]
        public string Url { get; set; }

        [Display(Name = "Icon")]
        [SchemaColumn]
        public string Icon { get; set; }

        [Display(Name = "描述")]
        [Required]
        [SchemaColumn]
        public string Description { get; set; }

        [Display(Name = "上级菜单")]
        [AutoComplete(DataType = "SystemMenu", Search = "Name", Label = "Name"), PlaceHolder("搜索名称")]
        [SchemaColumn(DisplayExpression = "Parent.Name")]
        public Guid? ParentMenuId { get; set; }

        [ForeignKey("ParentMenuId")]
        public SystemMenu Parent { get; set; }
    }
}
