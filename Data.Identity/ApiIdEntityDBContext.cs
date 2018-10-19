using Data.Entitys;
using Data.Identity;
using Data.Identity.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Identity
{
    public class ApiIdEntityDBContext : IdentityDbContext<SysUser, SysRole, Guid>
    {
        public ApiIdEntityDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in typeof(EntityBase).Assembly.GetTypes()){
                if (type.BaseType == typeof(EntityBase))
                {
                    modelBuilder.Entity(type);
                }
            }
            base.OnModelCreating(modelBuilder);
        }


    }
}

