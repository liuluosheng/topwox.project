using System;
using System.Collections.Generic;
using System.Text;
using X.Data.Entitys;

namespace Data.Entitys.Entitys
{
    public class Employees : EntityBase
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Area { get; set; }
        public string Position { get; set; }
    }
}
