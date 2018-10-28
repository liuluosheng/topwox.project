using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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

        [EnableQuery]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SysUser model)
        {
            if (TryValidateModel(model))
            {
                if (_userManager.Users.Any(p => p.UserName == model.UserName))
                {
                    return BadRequest("user already exists！");
                }
                var result = await _userManager.CreateAsync(model, model.PassWord);
                if (result.Succeeded)
                    return Ok(model);
                return BadRequest(result);
            }
            return BadRequest("model validation fails!");
        }
        [EnableQuery]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            if (await _userManager.FindByIdAsync(key.ToString()) is SysUser user)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return Ok();
                return BadRequest(result);
            }
            return BadRequest("not find user by key.");
        }

        [EnableQuery]
        [HttpPatch]
        public async Task<IActionResult> Patch(string key, [FromBody]JsonPatchDocument<SysUser> doc)
        {
            if (await _userManager.FindByIdAsync(key) is SysUser user)
            {
                doc.ApplyTo(user, p => { });
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                return BadRequest(result);
            }
            return BadRequest("not find user by key.");
        }

    }
}