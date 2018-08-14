using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Core.IServices;
using Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Ew.Api.Controllers
{
    public abstract class BaseController<T> : ODataController
        where T : EntityBase
    {
        protected readonly IBaseService<T> _service;
        public BaseController(IBaseService<T> service)
        {
            _service = service;
        }
        [EnableQuery]
        [HttpGet]
        [Description("查询")]
        public virtual IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [EnableQuery]
        [HttpPost]
        [Description("新建")]
        public virtual async Task<IActionResult> Post(T model)
        {
            return Ok(await _service.Put(model, true));
        }

        [EnableQuery]
        [HttpDelete]
        [Description("删除")]
        public virtual async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            return Ok((await _service.Delete(p => p.Id == key)) > 0);
        }
    }
}