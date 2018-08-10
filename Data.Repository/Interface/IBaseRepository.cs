using Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    /// <summary>
    /// 实体仓储模型的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IBaseRepository<TEntity>
        where TEntity : EntityBase
    {
        Task<TEntity> Update(TEntity entity, bool isCommit);
        Task<T> Update<T>(T entity, bool isCommit = true) where T : EntityBase;
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<int> Delete<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<int> Delete(Expression<Func<TEntity, bool>> predicate);
        Task<T> Put<T>(T entity, bool isCommit = true) where T : EntityBase;
        Task<TEntity> Put(TEntity entity, bool isCommit = true);
        Task<int> Commit();
    }

}