﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Attributes;
using Data.Attributes.Shema;

namespace Data.Entitys
{
    [GeneratedOdataController]
    public class Product : EntityBase
    {
        [Display(Name = "产品名称"), SchemaColumn]
        [Required]
        public string Name { get; set; }
        [Display(Name = "单重"), SchemaColumn(Searchable = false)]
        [Required]
        public decimal Weight { get; set; }
        [Display(Name = "产品描述")]
        public string Description { get; set; }

        [Display(Name = "采购人员")]
        [AutoComplete(DataType = "Employees", Search = "Name,PhoneNumber", Label = "Name"), PlaceHolder("搜索名称，联系电话")]
        [SchemaColumn(DisplayExpression = "Purchasing.Name")]
        public Guid? PurchasingId { get; set; }


        [ForeignKey("PurchasingId")]
        public Employees Purchasing { get; set; }
    }
}
