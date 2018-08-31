using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using X.Data.Entitys;

namespace X.Data.Dto
{
    public class TestUser : EntityBase
    {

        [Display(Name = "姓名")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "区域")]
        [Required]
        public string Area { get; set; }

        [Display(Name = "职位")]
        public string Position { get; set; }

    }
}
