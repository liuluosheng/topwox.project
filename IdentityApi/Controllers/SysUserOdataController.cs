using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebService.Core;
using WebService.Identity.Api.Data;

namespace WebService.Identity.Api.Controllers
{

    public class SysUserController : ODataController
    {
        private readonly UserManager<SysUser> _userManager;
        public SysUserController(UserManager<SysUser> userManager)
        {
            _userManager = userManager;
        }
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(string key) => Ok(await _userManager.FindByIdAsync(key));

        [EnableQuery]
        [HttpGet]
        public IActionResult Get() => Ok(_userManager.Users);
    }


}