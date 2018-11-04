using Topwox.Data.Entitys;
using Microsoft.EntityFrameworkCore;
using System;

namespace Topwox.WebService.Api
{
    public class TopwoxApiDBContext : DbContext
    {
        public TopwoxApiDBContext(DbContextOptions options) : base(options)
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

