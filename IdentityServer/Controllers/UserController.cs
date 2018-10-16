using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IdentityServer.Model;
using Data.Model;


namespace IdentityServer.Controllers
{
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<IActionResult> SaveUser(User user)
        {
            if (TryValidateModel(user))
            {
                if (_userManager.Users.Any(p => p.UserName == user.UserName))
                {
                    return BadRequest("user already exists！");
                }
                await _userManager.CreateAsync(user);
                return Ok();
            }

            return BadRequest();
        }

        public async Task<ActionResult> SaveUserRole(UserRoleParam param)
        {
            var role = await _roleManager.FindByNameAsync(param.RoleName);
            if (role == null)
            {
                if (Enum.TryParse(param.RoleName, out SystemRole _))
                {
                    
                    var desc = typeof(SystemRole).GetProperty(param.RoleName).GetCustomAttribute<DescriptionAttribute>()?.Description;
                    await _roleManager.CreateAsync(new Role { Id = Guid.NewGuid(), Name = param.RoleName, Description = desc });
                }
                else
                {
                    return BadRequest();
                }
            }

            var user = await _userManager.FindByIdAsync(param.UserId);
            if (param.Checked)
                await _userManager.AddToRoleAsync(user, param.RoleName);
            else
                await _userManager.RemoveFromRoleAsync(user, param.RoleName);

            return Ok();
        }


        public class UserRoleParam
        {
            public string UserId { get; set; }
            public bool Checked { get; set; }
            public string RoleName { get; set; }
        }


    }
}