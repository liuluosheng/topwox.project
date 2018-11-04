using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topwox.IdentityServer.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Topwox.IdentityServer.ODataControllers
{

    public class UserController : ODataController
    {
        public readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
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
        public async Task<IActionResult> Post([FromBody]User model)
        {
            if (TryValidateModel(model))
            {
                if (_userManager.Users.Any(p => p.UserName == model.UserName))
                {
                    return BadRequest("user already exists！");
                }
                var result = await _userManager.CreateAsync(model, model.PasswordHash);
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
            if (await _userManager.FindByIdAsync(key.ToString()) is User user)
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
        public async Task<IActionResult> Patch(string key, [FromBody]JsonPatchDocument<User> doc)
        {
            if (await _userManager.FindByIdAsync(key) is User user)
            {
                doc.ApplyTo(user, p => { });
                var result = await _userManager.UpdateAsync(user);
                if (!string.IsNullOrEmpty(user.Password))
                {
                    //重置用户密码
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, token, user.Password);
                }
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                return BadRequest(result);
            }
            return BadRequest("not find user by key.");
        }
        [HttpGet]
        [ODataRoute("SysUser({id})/Claim")]
        public async Task<IActionResult> GetClaim(Guid key)
        {
            if (await _userManager.FindByIdAsync(key.ToString()) is User user)
            {
                return Ok(await _userManager.GetClaimsAsync(user));
            }
            return BadRequest("not find user by key.");
        }

    }
}