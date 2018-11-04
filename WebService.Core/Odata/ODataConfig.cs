
using Topwox.Data.Entitys;
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
using Topwox.Data.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace WebService.Core
{
    public static class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(p => p.GetTypes());
            foreach (var type in types)
            {
                if (type.GetCustomAttributes().Any(p => p is OdataEntity))
                {
                    var entityType = builder.AddEntitySet(type.Name, builder.AddEntityType(type));
                    /// [NotMapped] 的属性Odata默认不序列化
                    /// 当加上[DataMember]时强制进行序列化
                    foreach (var prop in type.GetProperties().Where(p => p.GetCustomAttribute<NotMappedAttribute>() != null && p.GetCustomAttribute<DataMemberAttribute>() != null))
                    {
                        entityType.EntityType.AddProperty(prop);
                    }
                }
            }

            return builder.GetEdmModel();
        }
    }
}
