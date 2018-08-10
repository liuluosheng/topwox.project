using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsToTs;
using Data.Entitys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Ew.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/schema")]
    public class SchemaController : Controller
    {
        [HttpGet]
        public ActionResult Get(string type = "")
        {
            var types = Assembly.Load("Data.Dto").GetTypes();
            var targetType = types.FirstOrDefault(p => p.Name.ToLower() == type.ToLower());
            if (targetType == null) return NotFound();


            //  var options = new TypeScriptOptions { UseInterfaceForClasses = p => true, };
            //  var ts = Generator.GenerateTypeScript(targetType, options);

            return Json(new Schema(targetType));
        }
    }

}