﻿using Data.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebService.Identity.Api.Data
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

