
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

namespace Ew.Api.Config
{
    public static class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("Users");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Employees>("Employees");

            var user= builder.EntityType<User>();
            return builder.GetEdmModel();
        }
    }

    public class CustomPropertyRoutingConvention : NavigationSourceRoutingConvention
    {
        private const string ActionName = "GetProperty";

        public override string SelectAction(RouteContext routeContext, SelectControllerResult controllerResult, IEnumerable<ControllerActionDescriptor> actionDescriptors)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}
