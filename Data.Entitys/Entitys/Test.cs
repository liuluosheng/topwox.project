﻿using Data.Attributes;
using Data.Attributes.Shema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entitys
{
    [GeneratedOdataController]
    public class Test : EntityBase
    {
        [Display(Name = "名称")]
        [SchemaColumn]
        public string Name { get; set; }

        [SchemaColumn]
        [Display(Name = "年纪")]
        public int Age { get; set; }

        [Display(Name = "员工")]
        [AutoComplete(DataType = "Employees", Search = "Name,PhoneNumber", Label = "Name"), PlaceHolder("搜索名称，联系电话")]
        [SchemaColumn(DisplayExpression = "Employees.Name",Searchable =false)]
        public Guid EmployeesId { get; set; }

        public Employees Employees { get; set; }
    }
}
