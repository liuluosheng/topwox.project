using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.Core.IServices;
using X.Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData.Routing;

namespace Ew.Api.Controllers
{
  
    public class UsersController : BaseController<User>
    {
        public UsersController(IBaseService<User> userService) : base(userService)
        {
        }
        [EnableQuery]
        [HttpGet]
        public IActionResult Page()
        {
            return Ok("page");
        }
    }
}
