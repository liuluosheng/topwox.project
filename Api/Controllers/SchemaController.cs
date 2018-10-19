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

namespace Ew.Api.Controllers
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
        public ActionResult Get(string type = "")
        {
            var types = typeof(EntityBase).Assembly.GetTypes();
            var targetType = types.FirstOrDefault(p => p.Name.ToLower() == type.ToLower());
            if (targetType == null) return NotFound();
            return Ok(new ControlFactory(_configuration).CreateSchema(targetType));
        } 
                
        [HttpGet("api/ts/{type}")]
        public ActionResult GetTS(string type = "")
        {
            var types = typeof(EntityBase).Assembly.GetTypes();
            var targetType = types.FirstOrDefault(p => p.Name.ToLower() == type.ToLower());
            if (targetType == null) return NotFound();
            return Content(Generator.GenerateTypeScript(targetType,
                new TypeScriptOptions { UseInterfaceForClasses = p => true }));
        }
    }

}