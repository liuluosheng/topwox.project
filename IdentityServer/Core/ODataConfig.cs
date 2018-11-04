
using Data.Entitys;
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
using Microsoft.AspNetCore.Mvc;
using Data.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using IdentityServer.Model;

namespace IdentityServer.Config
{
    public static class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("User")
                .EntityType.Property(p => p.Password);
            builder.EntitySet<Role>("Role");
            return builder.GetEdmModel();
        }
    }
}
