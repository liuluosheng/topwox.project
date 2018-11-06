using System;
using System.Collections.Generic;
using System.Text;

namespace Topwox.Data.Entitys.Base
{
    /// <summary>
    /// 继承此接口实现软删除
    /// </summary>
    public class ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}

/*
 * 在DbContext类中做如下定义，以实现软删除
 protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in typeof(EntityBase).Assembly.GetTypes())
            {
                if (type.BaseType == typeof(EntityBase))
                {

                    modelBuilder.Entity(type);
                    if (typeof(ISoftDelete).IsAssignableFrom(t))
                    {
                        var columnName = "IsDeleted";
                        modelBuilder.Entity(type).Property<bool>(columnName);
                        Expression<Func<ISoftDelete, bool>> expression = e => !e.IsDeleted;
                        modelBuilder.Entity(type).HasQueryFilter(expression);
                    }
                }
            }
            base.OnModelCreating(modelBuilder);
        }
     
 */
