using Data.Entitys;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class ApiDBContext : DbContext
    {
        public ApiDBContext(DbContextOptions options) : base(options)
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

