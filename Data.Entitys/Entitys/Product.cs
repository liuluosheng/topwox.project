using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Topwox.Data.Attributes;
using Topwox.Data.Attributes.Shema;

namespace Topwox.Data.Entitys
{
    [GeneratedOdataController]
    public class Product : EntityBase
    {
        [Display(Name = "产品名称")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "单重")]
        [Required]
        public decimal Weight { get; set; }
        [Display(Name = "产品描述")]
        public string Description { get; set; }

        [Display(Name = "采购人员")]
        [AutoComplete(DataType = "Employees", Search = "Name,PhoneNumber", Label = "Name"), PlaceHolder("搜索名称，联系电话")]
        [NavigationExpression("Purchasing.Name")]
        public Guid? PurchasingId { get; set; }


        [ForeignKey("PurchasingId")]
        public Employees Purchasing { get; set; }
    }
}
