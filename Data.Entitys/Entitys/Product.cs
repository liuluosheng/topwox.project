using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace X.Data.Entitys
{
    
  public  class Product:EntityBase
    {
        [Display(Name = "产品名称")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "单重")]
        [Required]
        public decimal Weight { get; set; }
        [Display(Name = "产品描述")]
        public string Description { get; set; }

        public Guid? PurchasingId { get; set; }

        [Display(Name = "采购人员")]
        [ForeignKey("PurchasingId")]
        public Employees Purchasing { get; set; }
    }
}
