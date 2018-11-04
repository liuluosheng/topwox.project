using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsToTs;
using Topwox.Data.Model;
using Topwox.Data.Entitys;
using Microsoft.Extensions.Configuration;
using Topwox.Data.Model.Control;
using CacheCow.Server.Core.Mvc;
using EasyCaching.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.ComponentModel.DataAnnotations;
using Topwox.WebService.Core.Authorization;
using EnumsNET;
using Module = Topwox.WebService.Core.Authorization.Module;
using Microsoft.AspNetCore.Authorization;

namespace Topwox.WebService.Core.Controllers
{
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly IEasyCachingProvider _provider;
        public SchemaController(IConfiguration configuration, IEasyCachingProvider provider)
        {
            _configuration = configuration;
            _provider = provider;
        }
        /// <summary>
        /// 构建一个JSON Schema 供前端调用生成Form
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpCacheFactory(300)]
        [Authorize]
        [ApiAuthorize(typeof(Employees), Operation.Update | Operation.Delete)]
        [HttpGet("api/jsonschema/{type}")]
        public IActionResult Get(string type = "")
        {
            var targetType = GetType(type);
            if (targetType == null) return NotFound();
            return Ok(new ControlFactory(_configuration).CreateSchema(targetType));
        }
        /// <summary>
        /// 返回指定类的Typescript定义
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("api/ts/{type}")]
        public IActionResult GetTS(string type = "")
        {

            var targetType = GetType(type);
            if (targetType == null) return NotFound();
            return Ok(Generator.GenerateTypeScript(targetType, new TypeScriptOptions { UseInterfaceForClasses = p => true }));
        }
        private IEnumerable<Type> GetTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(p => p.GetTypes());
        }
        private Type GetType(string type)
        {
            if (GetTypes().FirstOrDefault(p => p.Name.ToLower() == type.ToLower()) is Type t)
            {
                return t;
            }
            return null;
        }
        /// <summary>
        /// 返回接口的定义信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("api/getapidescription")]
        public IActionResult GetApiInfo()
        {
            var actionCollectionProvider = HttpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();
            List<Module> modules = new List<Module>();
            foreach (var api in actionCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>())
            {
                if (api.MethodInfo.GetCustomAttribute<ApiAttribute>() is ApiAttribute t)
                {
                    string module = api.ControllerName;
                    string descripions = api.ControllerName;
                    if (api.ControllerTypeInfo.IsGenericType)
                    {
                        var genericType = api.ControllerTypeInfo.GenericTypeArguments.FirstOrDefault() ?? api.ControllerTypeInfo.BaseType.GenericTypeArguments.FirstOrDefault();
                        if (genericType != null)
                        {
                            module = genericType.Name;
                            descripions = genericType.GetCustomAttribute<DescriptionAttribute>()?.Description
                               ?? genericType.Name;
                        }
                    }
                    var m = modules.FirstOrDefault(p => p.Key == module) ?? new Module { Key = module, Descripton = descripions };
                    if (m.Operations.Any(p => p.Operation == t.Operation)) continue;
                    var member = t.Operation.GetMember();
                    IEnumerable<Operation> data = member != null ? data = new List<Operation> { t.Operation } : null;
                    foreach (var flag in data ?? t.Operation.GetFlags())
                    {
                        if (m.Operations.Any(p => p.Operation == flag)) continue;
                        string desc = flag.ToString();
                        var description = Enums.GetMember(flag).Attributes.FirstOrDefault(p => p is DescriptionAttribute);
                        if (description != null)
                            desc = ((DescriptionAttribute)description).Description;
                        m.Operations.Add(new OperationDescription { Description = desc, Operation = flag });
                    }

                    if (modules.All(p => p.Key != m.Key))
                    {
                        modules.Add(m);
                    }
                }
            }
            return Ok(modules);
        }
    }

}
