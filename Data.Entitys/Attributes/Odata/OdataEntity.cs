using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Attributes
{
    /// <summary>
    //标识实体添加为Odata的edm的entityType
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OdataEntity : Attribute
    {
        public OdataEntity(string route=null)
        { 
        }    
    }
}
