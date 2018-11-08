using Topwox.Data.Entitys;
using Microsoft.EntityFrameworkCore;
using System;
using Topwox.Data.Entitys.Base;
using System.Linq.Expressions;

namespace Topwox.WebService.Api
{
    public class TopwoxApiDBContext : DbContext
    {
        public TopwoxApiDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in typeof(EntityBase).Assembly.GetTypes())
            {
                if (type.BaseType == typeof(EntityBase))
                {

                    modelBuilder.Entity(type);
                    if (typeof(ISoftDelete).IsAssignableFrom(type))
                    {
                        var columnName = "IsDeleted";
                        modelBuilder.Entity(type).Property<bool>(columnName);
                        var param = Expression.Parameter(type, "p");
                        modelBuilder.Entity(type).HasQueryFilter(Expression.Lambda(Expression.NotEqual(Expression.Property(param, columnName), Expression.Constant(false)), param));
                    }
                }
            }
            base.OnModelCreating(modelBuilder);
        }


    }
}

