
using X.Data.Entitys;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNet.OData.Routing;
using System.Reflection;

namespace Ew.Api.Core
{
    public static class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            var types = typeof(EntityBase).Assembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsAbstract && type.BaseType == typeof(EntityBase))
                {
                    builder.AddEntitySet(type.Name, builder.AddEntityType(type));
                }
            }
            //builder.EntitySet<User>("Users");
            //builder.EntitySet<Supplier>("Suppliers");
            //builder.EntitySet<Product>("Products");
            //builder.EntitySet<Employees>("Employees");
            //var user = builder.EntityType<User>();
            return builder.GetEdmModel();
        }
    }
}
