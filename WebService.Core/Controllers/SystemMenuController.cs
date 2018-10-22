using Core.IServices;
using Data.Entitys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebService.Core.Controllers;
using Newtonsoft.Json;

namespace WebService.Core
{
    public class SystemMenuController : BaseController<SystemMenu>
    {
        public SystemMenuController(IBaseService<SystemMenu> service) : base(service)
        {
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(JsonConvert.SerializeObject(_service.Get(p => p.Id == id, "Items").First(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

     

    }
}
