using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ew.IdentityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/identity")]
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Json(HttpContext.User.Identity);
        }
    }
}