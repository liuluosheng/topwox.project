
using Topwox.Data.Entitys;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using Topwox.IdentityServer.Model;

namespace Topwox.IdentityServer.Config
{
    public static class IdentityServerODataConfig
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
