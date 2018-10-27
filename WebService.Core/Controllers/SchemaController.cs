using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsToTs;
using Data.Model;
using Data.Entitys;
using Microsoft.Extensions.Configuration;
using Data.Model.Control;
using CacheCow.Server.Core.Mvc;
using EasyCaching.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.ComponentModel.DataAnnotations;

namespace WebService.Core.Controllers
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
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(var api in actionCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>())
            {
                if(api.MethodInfo.GetCustomAttribute<ApiAttribute>() is ApiAttribute t)
                {
                    string displayname = api.ControllerName;
                    var generic = api.ControllerTypeInfo.IsGenericType
                        ? api.ControllerTypeInfo.GenericTypeArguments.FirstOrDefault()
                        :  api.ControllerTypeInfo.BaseType.GenericTypeArguments.FirstOrDefault();
                    if (generic!=null)
                    {
                      displayname=  generic.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                            ?? generic.GetCustomAttribute<DisplayAttribute>()?.Name
                            ?? generic.Name;
                    }              
                    result.Add($"{api.ControllerName}.{api.ActionName}", $"{displayname} - {t.Description}");
                    
                }
            }
            return Ok(result);
        }
    }

}