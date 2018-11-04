using System;
using System.Collections.Generic;
using System.Text;
using Topwox.WebService.Core.Authorization;

namespace Topwox.WebService.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiAttribute : Attribute
    {
        public ApiAttribute(Operation operation)
        {
            Operation = operation;
        }
        public Operation Operation { get; set; }     
    }

}
