using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.Core.IServices;
using X.Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using X.Data.Dto;

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
        public virtual IActionResult Get(Guid key)
        {
            return Ok(_service.Get(p => p.Id == key));
        }

        [EnableQuery]
        [HttpGet]
        public virtual IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [EnableQuery]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody]T model)
        {

            return Ok(await _service.Put(model));

        }

        [EnableQuery]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            return Ok((await _service.Delete(p => p.Id == key)) > 0);
        }
    }
}