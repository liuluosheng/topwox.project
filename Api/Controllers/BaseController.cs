using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.Core.IServices;
using X.Data.Entitys;
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
        public virtual IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [EnableQuery]
        [HttpPost]
        public virtual async Task<IActionResult> Post(T model)
        {
            return Ok(await _service.Put(model, true));
        }

        [EnableQuery]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            return Ok((await _service.Delete(p => p.Id == key)) > 0);
        }
    }
}