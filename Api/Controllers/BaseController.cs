using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.IServices;
using Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Ew.Api.Controllers
{
    public abstract class BaseController<T> : ODataController where T : EntityBase
    {
        protected IBaseService<T> _service;
        public BaseController(IBaseService<T> service)
        {
            _service = service;
        }
        public IActionResult Get()
        {
            return Ok(_service)
        }
    }
}