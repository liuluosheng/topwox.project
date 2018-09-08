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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Data.Edm;
using ODataPath = Microsoft.AspNet.OData.Routing.ODataPath;
using Microsoft.OData.UriParser;

namespace Ew.Api.Controllers
{
    public  class BaseController<T> : ODataController
        where T : EntityBase
    {
        protected readonly IBaseService<T> _service;
        public BaseController(IBaseService<T> service)
        {
            _service = service;
        }
        [EnableQuery]
        public virtual IActionResult Get(Guid key) => Ok(_service.Get(p => p.Id == key));

        [EnableQuery]
        [HttpGet]
        public virtual IActionResult Get() => Ok(_service.Get());

        [EnableQuery]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody]T model) => Ok(await _service.Create(model));

        [EnableQuery]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody]T model) => Ok(await _service.Update(model));

        [EnableQuery]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromODataUri] Guid key) => Ok((await _service.Delete(p => p.Id == key)) > 0);

        [EnableQuery]
        [HttpPatch]
        public virtual async Task<IActionResult> Patch(Guid key, [FromBody]JsonPatchDocument<T> doc) => Ok( await _service.Patch(key, doc));

        public IActionResult GetNavigation(string key, string navigation)
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

            //IEdmEntityType entityType = property.NavigationProperty.DeclaringType as IEdmEntityType;

            //EdmEntityObject entity = new EdmEntityObject(entityType);

            //string sourceString = Request.GetDataSource();
            //DataSourceProvider.Get(sourceString, key, entity);

            //object value = DataSourceProvider.GetProperty(sourceString, navigation, entity);

            //if (value == null)
            //{
            //    return NotFound();
            //}

            //IEdmEntityObject nav = value as IEdmEntityObject;
            //if (nav == null)
            //{
            //    return NotFound();
            //}

            return Ok(navigation+key.ToString());
        }
    }
}