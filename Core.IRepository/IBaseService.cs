﻿using Data.Entitys;
using Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IBaseService<TEntity> where TEntity : EntityBase
    {
        IQueryable<TEntity> GetPaging();
        Task<TEntity> Update(TEntity entity, bool isCommit);
        Task<T> Update<T>(T entity, bool isCommit = true) where T : EntityBase;
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<int> Delete<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;
        Task<int> Delete(Expression<Func<TEntity, bool>> predicate);
        Task<T> Put<T>(T entity, bool isCommit = true) where T : EntityBase;
        Task<TEntity> Put(TEntity entity, bool isCommit = true);
    }


}