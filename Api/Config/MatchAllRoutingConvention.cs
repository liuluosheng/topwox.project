// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.UriParser;

namespace Ew.Api.Config
{
    public class CustomPropertyRoutingConvention : NavigationSourceRoutingConvention
    {

        public override string SelectAction(RouteContext routeContext, SelectControllerResult controllerResult, IEnumerable<ControllerActionDescriptor> actionDescriptors)
        {
            var odataPath = routeContext.HttpContext.ODataFeature().Path;

            IActionDescriptorCollectionProvider actionCollectionProvider =
                routeContext.HttpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();
            if (odataPath.PathTemplate == "~/entityset/key/navigation")
            {
                if (routeContext.HttpContext.Request.Method.ToUpperInvariant() == "GET")
                {
                    NavigationPropertySegment navigationPathSegment = (NavigationPropertySegment)odataPath.Segments.Last();

                    routeContext.RouteData.Values["navigation"] = navigationPathSegment.NavigationProperty.Name;

                    KeySegment keyValueSegment = (KeySegment)odataPath.Segments[1];
                    routeContext.RouteData.Values[ODataRouteConstants.Key] = keyValueSegment.Keys.First().Value;

                    return "GetNavigation";
                }
            }
            return null;
        }
    }
}
