﻿
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topwox.Data.Entitys;
using System.Reflection;
using Topwox.Data.Attributes;

namespace Topwox.WebService.Core
{
    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(p => p.GetTypes());
            var candidates = types.Where(x => x.GetCustomAttributes<GeneratedOdataControllerAttribute>().Any());
            foreach (var candidate in candidates)
            {
                feature.Controllers.Add(
                    typeof(BaseOdataController<>).MakeGenericType(candidate).GetTypeInfo()
                );
            }
        }
    }
}
