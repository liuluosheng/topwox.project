// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Edm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
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
    public class MatchRoutingConvention : IODataRoutingConvention
    {
        private string ControllerName = "";

        public IEnumerable<ControllerActionDescriptor> SelectAction(RouteContext routeContext)
        {

            var odataPath = routeContext.HttpContext.ODataFeature().Path;
            if (!(odataPath.Segments.FirstOrDefault() is EntitySetSegment))
            {
                return Enumerable.Empty<ControllerActionDescriptor>();
            }
            EdmCollectionType collectionType = (EdmCollectionType)odataPath.EdmType;
            Microsoft.OData.Edm.IEdmEntityTypeReference entityType = collectionType.ElementType.AsEntity();
            var type = Assembly.Load("X.Data.Entitys").CreateInstance(entityType.Definition.ToString()).GetType();
            ControllerName = type.Name;
            // Get a IActionDescriptorCollectionProvider from the global service provider.
            IActionDescriptorCollectionProvider actionCollectionProvider =
                routeContext.HttpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();

            IEnumerable<ControllerActionDescriptor> actionDescriptors = actionCollectionProvider
                    .ActionDescriptors.Items.OfType<ControllerActionDescriptor>()
                    .Where(c => c.ControllerName == ControllerName);

            if (odataPath.PathTemplate == "~/entityset/key/navigation")
            {
                if (routeContext.HttpContext.Request.Method.ToUpperInvariant() == "GET")
                {
                    NavigationPropertySegment navigationPathSegment = (NavigationPropertySegment)odataPath.Segments.Last();

                    routeContext.RouteData.Values["navigation"] = navigationPathSegment.NavigationProperty.Name;

                    KeySegment keyValueSegment = (KeySegment)odataPath.Segments[1];
                    routeContext.RouteData.Values[ODataRouteConstants.Key] = keyValueSegment.Keys.First().Value;

                    return actionDescriptors.Where(c => c.ActionName == "GetNavigation");
                }
            }

            SelectControllerResult controllerResult = new SelectControllerResult(ControllerName, null);
            IList<IODataRoutingConvention> routingConventions = ODataRoutingConventions.CreateDefault();
            foreach (NavigationSourceRoutingConvention nsRouting in routingConventions.OfType<NavigationSourceRoutingConvention>())
            {
                string actionName = nsRouting.SelectAction(routeContext, controllerResult, actionDescriptors);
                if (!String.IsNullOrEmpty(actionName))
                {
                    return actionDescriptors.Where(
                        c => String.Equals(c.ActionName, actionName, StringComparison.OrdinalIgnoreCase));
                }
            }

            return null;
        }
    }
}
