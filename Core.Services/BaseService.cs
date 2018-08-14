using Core.IServices;
using Data.Entitys;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interface;
using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;

namespace Core.Service
{
    /// <summary>
    /// Service基类
    /// </summary>
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : EntityBase
    {
        protected readonly IBaseRepository _repository;
        public BaseService(IBaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Delete<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            return await _repository.Delete<T>(predicate);
        }

        public async Task<int> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return await Delete<TEntity>(predicate);
        }

        public IQueryable<TEntity> Get()
        {
            return _repository.Get<TEntity>();
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Get<TEntity>(predicate);
        }
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            return _repository.Get<T>(predicate);
        }

        public IQueryable<TEntity> GetPaging()
        {
            throw new NotImplementedException();
        }

        public async Task<T> Put<T>(T entity, bool isCommit = true) where T : EntityBase
        {
            return await _repository.Put<T>(entity, isCommit);
        }

        public async Task<TEntity> Put(TEntity entity, bool isCommit = true)
        {
            return await Put<TEntity>(entity, isCommit);
        }

        public async Task<TEntity> Put(Guid id, JsonPatchDocument<TEntity> doc, bool isCommit = true)
        {
            var model = await Get(p => p.Id == id).FirstOrDefaultAsync();
            doc.ApplyTo(model, p => { });
            return await Put(model, isCommit);
        }

        public async Task<TEntity> Update(TEntity entity, bool isCommit)
        {
            return await Update<TEntity>(entity, isCommit);
        }

        public async Task<T> Update<T>(T entity, bool isCommit = true) where T : EntityBase
        {
            return await _repository.Update<T>(entity, isCommit);
        }
    }
}
