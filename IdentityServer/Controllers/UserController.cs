using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ew.IdentityServer.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Data.Utility;
using System.Security.Claims;

namespace Ew.IdentityServer.Controllers
{
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Authorize(string id)
        {
            ViewBag.UserId = id;
            return View();
        }

        public ActionResult Page([FromBody]Page page)
        {
            page.PageIndex--;
            return Json(new
            {
                rows = _userManager.Users.Skip(page.PageIndex * page.PageSize).Take(page.PageSize).ToList(),
                count = _userManager.Users.Count()
            });
        }

        public async Task<ActionResult> UpdateClaim([FromBody]UpdateClaimData param)
        {
            var user = await _userManager.FindByIdAsync(param.UserId);
            var claim = new Claim(param.Action.ClaimType ?? "api_authorize", param.Action.Name);
            if (param.Checked)
                await _userManager.AddClaimAsync(user, claim);
            else
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }
            return Ok();
        }
        public class UpdateClaimData
        {
            public string UserId { get; set; }
            public bool Checked { get; set; }
            public ApiShemaAction Action { get; set; }
        }


    }
}