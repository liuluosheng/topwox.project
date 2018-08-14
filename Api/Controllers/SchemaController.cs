using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsToTs;
using Data.Entitys;
using Data.Utility;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Ew.Api.Controllers
{
    public class SchemaController : Controller
    {
        /// <summary>
        /// 构建一个JSON Schema 供前端调用生成Form
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("api/jsonschema/{type}")]
        public ActionResult Get(string type = "")
        {
            var types = Assembly.Load("Data.Dto").GetTypes();
            var targetType = types.FirstOrDefault(p => p.Name.ToLower() == type.ToLower());
            if (targetType == null) return NotFound();
            return Json(new JsonSchema(targetType));
        }
        /// <summary>
        /// 构建授权需要的Schema供授权服务器调用
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/schema")]
        public IActionResult Schema()
        {
            var types = Assembly.GetAssembly(typeof(BaseController<>)).GetTypes();
            var baseType = typeof(BaseController<>);
            var data = new Dictionary<string, object>();
            var apiSchema = new ApiSchema { Name = "Api的描述信息", Modules = new List<ApiSchemaModule>() };
            foreach (var type in types)
            {
                if (type.BaseType.BaseType == typeof(ODataController))
                {
                    var name = type.Name.Replace("Controller", "");
                    var apiModule = new ApiSchemaModule
                    {
                        Name = name,
                        Actions = new List<ApiShemaAction>(),
                        Description = type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "没有描述信息！"
                    };
                    foreach (var method in type.GetMethods())
                    {
                        if (method.GetCustomAttribute<EnableQueryAttribute>() != null)
                        {
                            apiModule.Actions.Add(new ApiShemaAction
                            {
                                Name = $"Api.{name}.{method.Name}",
                                ClaimType = "api_authorize",
                                Description = method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? method.Name
                            });
                        }
                    }
                    if (apiModule.Actions.Any())
                    {
                        apiSchema.Modules.Add(apiModule);
                    }
                }
            }
            return Json(apiSchema);
        }
    }

}