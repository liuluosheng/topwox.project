﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EnumsNET;
namespace WebService.Core.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public ApiAuthorizeAttribute(PrivateOperation operation)
        {
            Operation = operation;
        }
        public PrivateOperation Operation { get; set; }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
           
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new PermissionAuthorizationRequirement(Operation));
            if (!authorizationResult.Succeeded)
            {         
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
