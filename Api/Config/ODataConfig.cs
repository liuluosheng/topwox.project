
using X.Data.Entitys;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ew.Api.Config
{
    public static class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("Users");
            
           var user= builder.EntityType<User>();
            return builder.GetEdmModel();
        }
    }
}
