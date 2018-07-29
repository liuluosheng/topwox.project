

using Data;
using Data.Entitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ew.IdentityServer
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }

    /// <summary>
    /// 创建种子数据
    /// </summary>
    public class DatabaseInitializer : IDatabaseInitializer
    {
        ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public DatabaseInitializer(ApplicationDbContext dbcontext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = dbcontext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            if (!await _context.Users.AnyAsync())
            {
                var userId = Guid.NewGuid().ToString();
                var roleId = Guid.NewGuid().ToString();
                User user = new User
                {
                    Id = userId,
                    PhoneNumber = "13534138066",
                    UserName = "346642458@qq.com",
                    Email = "346642458@qq.com"
                };
                Role role = new Role
                {
                    Id = roleId,
                    Name = "Admin",
                    //Permissions = JsonConvert.SerializeObject(new Dictionary<Module, Operation> {
                    //     { Module.产品中心,Operation.查询 },
                    //     { Module.用户中心,Operation.创建 | Operation.删除 },
                    //     { Module.关于我们,  Operation.查询 | Operation.创建  }
                    //}, 
                    //new StringEnumConverter())
                };
                var identity = await _userManager.CreateAsync(user, "asdf-123");
                if (identity.Succeeded)
                {
                    await _roleManager.CreateAsync(role);
                    user = await _userManager.FindByNameAsync(user.UserName);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

            }
        }
    }
}
