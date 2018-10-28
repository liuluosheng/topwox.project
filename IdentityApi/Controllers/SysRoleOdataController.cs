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

    public class SysRoleController : ODataController
    {
        private readonly RoleManager<SysRole> _roleManager;
        public SysRoleController(RoleManager<SysRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(string key) => Ok(await _roleManager.FindByIdAsync(key));

        [EnableQuery]
        [HttpGet]
        public IActionResult Get() => Ok(_roleManager.Roles);

        [EnableQuery]
        [HttpPost]
        public  async Task<IActionResult> Post([FromBody]SysRole model)
        {
            if (TryValidateModel(model))
            {
                if (_roleManager.Roles.Any(p => p.Name == model.Name))
                {
                    return BadRequest("role already exists！");
                }
                var result = await _roleManager.CreateAsync(model);
                if (result.Succeeded)
                {
                    return Ok(model);
                }
                return BadRequest(result);
            }
            return BadRequest("model validation fails!");
        }
        [EnableQuery]
        [HttpDelete]
        public  async Task<IActionResult> Delete([FromODataUri] string key)
        {
            if (await _roleManager.FindByIdAsync(key) is SysRole role)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Ok(role);
                }
                return BadRequest(result);
            }
            return BadRequest("not find role by key.");
        }

        [EnableQuery]
        [HttpPatch]
        public  async Task<IActionResult> Patch(string key, [FromBody]JsonPatchDocument<SysRole> doc)
        {
            if (await _roleManager.FindByIdAsync(key) is SysRole role)
            {
                doc.ApplyTo(role, p => { });
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(role);
                }
                return BadRequest(result);
            }
            return BadRequest("not find role by key.");
        }

    }
}