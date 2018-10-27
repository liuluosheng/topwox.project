using System;
using System.Collections.Generic;
using System.Text;

namespace WebService.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ApiAttribute : Attribute
    {
        public ApiAttribute(string description)
        {
            Description = description;
        }
        public string Description { get; set; }
        public  Type T { get; set; }
    }

}
