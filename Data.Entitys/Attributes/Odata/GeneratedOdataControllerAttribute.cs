using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Topwox.Data.Attributes
{
    /// <summary>
    /// 动态构建OData的Controller
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GeneratedOdataControllerAttribute : OdataEntity
    {
        public GeneratedOdataControllerAttribute(string route=null)
        {
            Route = route;
        }

        public string Route { get; set; }
    }
}
