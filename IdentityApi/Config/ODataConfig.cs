﻿
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
using WebService.Identity.Api.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace WebService.Identity.Api
{
    public static class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            var types = typeof(EntityBase).Assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.BaseType == typeof(EntityBase) && type.GetCustomAttribute<GeneratedOdataControllerAttribute>() is GeneratedOdataControllerAttribute)
                {
                  var entityType=  builder.AddEntitySet(type.Name, builder.AddEntityType(type));
                    /// [NotMapped] 的属性Odata默认不序列化
                    /// 当加上[DataMember]时强制进行序列化
                    foreach (var prop in type.GetProperties().Where(p => p.GetCustomAttribute<NotMappedAttribute>() != null && p.GetCustomAttribute<DataMemberAttribute>() != null))
                    {
                        entityType.EntityType.AddProperty(prop);
                    }
                }
            }
            builder.EntitySet<SysUser>("SysUser")
                .EntityType.Property(p => p.Password);
            builder.EntitySet<SysRole>("SysRole");

            return builder.GetEdmModel();
        }
    }
}
