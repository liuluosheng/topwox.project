using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace Data.Entitys
{
    public class Role : EntityBase
    {
        public string Permissions { get; set; }


    }
}
