using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Utility
{

    public class ApiSchema
    {
        /// <summary>
        /// Api名称
        /// </summary>
        public string Name { get; set; }
        public List<ApiSchemaModule> Modules { get; set; }
    }

    public class ApiSchemaModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ApiShemaAction> Actions { get; set; }
    }

    public class ApiShemaAction
    {
        public string Name { get; set; }
        public string ClaimType { get; set; }
        public string Description { get; set; }
    }
}
