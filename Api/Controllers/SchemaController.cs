using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using X.Data.Utility;
using CsToTs;
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
            var types = Assembly.Load("X.Data.Dto").GetTypes();
            var targetType = types.FirstOrDefault(p => p.Name.ToLower() == type.ToLower());
            if (targetType == null) return NotFound();
            return Json(new JsonSchema(targetType));
        }
        [HttpGet("api/ts/{type}")]
        public ActionResult GetTS(string type = "")
        {
            var types = Assembly.Load("X.Data.Dto").GetTypes();
            var targetType = types.FirstOrDefault(p => p.Name.ToLower() == type.ToLower());
            if (targetType == null) return NotFound();
            return Content(Generator.GenerateTypeScript(targetType,
                new TypeScriptOptions { UseInterfaceForClasses = p => true }));
        }
    }

}