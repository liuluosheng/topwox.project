using X.Data.Entitys;
using X.Data.Repository.Interface;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace X.Core.IServices
{
    public interface IBaseService<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> Update(TEntity entity, bool isCommit);
        Task<T> Update<T>(T entity, bool isCommit = true) where T : EntityBase;
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<int> Delete<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<int> Delete(Expression<Func<TEntity, bool>> predicate);
        Task<T> Create<T>(T entity, bool isCommit = true) where T : EntityBase;
        Task<TEntity> Patch(Guid id, JsonPatchDocument<TEntity> doc, bool isCommit = true);
        Task<TEntity> Create(TEntity entity, bool isCommit = true);
    }


}