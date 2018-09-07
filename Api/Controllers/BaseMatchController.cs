using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.Core.IServices;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using X.Data.Dto;
using User = X.Data.Entitys.User;
using Microsoft.AspNet.OData.Routing;
using System.Reflection;
using X.Data.Entitys;

namespace Ew.Api.Controllers
{
    public class BaseMatchController : ODataController
    {
        public BaseMatchController()
        {
            //string type = ControllerContext.RouteData.Values["type"].ToString();
           // var genericType = typeof(IBaseService<>).MakeGenericType(Assembly.Load("X.Data.Entitys").CreateInstance(type).GetType());
           // IBaseService<EntityBase> service = Request.HttpContext.RequestServices.GetService(genericType) as IBaseService<EntityBase>;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok("OK");
        }
    }
}
