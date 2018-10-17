using Core.IServices;
using Data.Entitys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// api controller 基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Route("api/[controller]")]
    public abstract class BaseController<T> : Controller
        where T : EntityBase
    {
        protected readonly IBaseService<T> _service;
        public BaseController(IBaseService<T> service)
        {
            _service = service;
        }
    }
}
