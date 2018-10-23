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
            return Ok(JsonConvert.SerializeObject(new Menu(_service, _service.Get(p => p.Id == id).First()), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        private class Menu
        {
            private IBaseService<SystemMenu> _service;
            public Menu(IBaseService<SystemMenu> service, SystemMenu menu)
            {
                Id = menu.Id;
                Name = menu.Name;
                Url = menu.Url;
                Icon = menu.Icon;
                _service = service;
            }
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public string Icon { get; set; }
            public List<Menu> Items
            {
                get
                {
                    return _service.Get(p => p.ParentMenuId == Id).Select(p => new Menu(_service, p)).ToList();
                }
            }
        }

    }
}
