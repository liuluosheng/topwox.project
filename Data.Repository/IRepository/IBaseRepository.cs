using Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Repository
{
    /// <summary>
    /// 实体仓储模型的数据标准操作
    /// </summary>
    public interface IBaseRepository

    {
        Task<T> Update<T>(T entity, bool isCommit = true) where T : EntityBase;
        IQueryable<T> Get<T>() where T : EntityBase;
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<int> Delete<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<T> Put<T>(T entity, bool isCommit = true) where T : EntityBase;
        Task<int> Commit();
    }

}