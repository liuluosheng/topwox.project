using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Core.IServices;
using Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Data.Edm;
using ODataPath = Microsoft.AspNet.OData.Routing.ODataPath;
using Microsoft.OData.UriParser;
using System.Reflection;
using CacheCow.Server.Core.Mvc;
using WebService.Core.Authorization;

namespace WebService.Core
{
    public class BaseOdataController<T> : ODataController
        where T : EntityBase
    {
        protected readonly IBaseService<T> _service;
        public BaseOdataController(IBaseService<T> service)
        {
            _service = service;
        }
        [EnableQuery]
        [Api(Operation.Read)]
        public virtual IActionResult Get(Guid key) => Ok(_service.Get(p => p.Id == key));

        [EnableQuery]
        [HttpGet]
        [Api(Operation.Read)]
        public virtual IActionResult Get() => Ok(_service.Get());

        [EnableQuery]
        [HttpPost]
        [Api(Operation.Create)]
        public virtual async Task<IActionResult> Post([FromBody]T model) => Ok(await _service.Create(model));

        [EnableQuery]
        [HttpPut]
        [Api(Operation.Update)]
        public virtual async Task<IActionResult> Put([FromBody]T model) => Ok(await _service.Update(model));

        [EnableQuery]
        [HttpDelete]
        [Api(Operation.Delete)]
        public virtual async Task<IActionResult> Delete([FromODataUri] Guid key) => Ok((await _service.Delete(p => p.Id == key)) > 0);

        [EnableQuery]
        [HttpPatch]
        [Api(Operation.Update)]
        public virtual async Task<IActionResult> Patch(Guid key, [FromBody]JsonPatchDocument<T> doc) => Ok(await _service.Patch(key, doc));


        public IActionResult GetNavigation(Guid key, string navigation)
        {
            ODataPath path = Request.ODataFeature().Path;
            if (path.PathTemplate != "~/entityset/key/navigation")
            {
                return BadRequest("Not the correct navigation property access request!");
            }

            if (!(path.Segments.Last() is NavigationPropertySegment property))
            {
                return BadRequest("Not the correct navigation property access request!");
            }

            T model = _service.Get(c => c.Id == key).First();
            if (model == null)
            {
                return BadRequest("Not find the model!");
            }
            PropertyInfo info = typeof(T).GetProperty(navigation);
            object value = info.GetValue(model);
            return Ok(value);
        }
    }
}