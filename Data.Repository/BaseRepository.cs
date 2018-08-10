using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Entitys;
using Data.Repository.Interface;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    /// <summary>
    /// EntityFramework的仓储实现
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> Update(TEntity entity, bool isCommit)
        {
            return await Update<TEntity>(entity, isCommit);
        }
        public async Task<T> Update<T>(T entity, bool isCommit = true) where T : EntityBase
        {
            await _context.Set<T>().AddAsync(entity);
            if (isCommit)
                await Commit();
            return entity;
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Get<TEntity>(predicate);
        }
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            return _context.Set<T>().Where(predicate);
        }
        public async Task<int> Delete<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            var entitys = await _context.Set<T>().Where(predicate).ToArrayAsync();
            _context.Set<T>().RemoveRange(entitys);
            return await Commit();
        }
        public async Task<int> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return await Delete<TEntity>(predicate);
        }
        public async Task<T> Put<T>(T entity, bool isCommit = true) where T : EntityBase
        {
            entity.Id = entity.Id == Guid.Empty
               ? EntityBase.NewId()
               : entity.Id;
            _context.Set<T>().Add(entity);
            if (isCommit)
                await Commit();
            return entity;
        }
        public async Task<TEntity> Put(TEntity entity, bool isCommit = true)
        {
            return await Put<TEntity>(entity, isCommit);
        }
        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }



    }
}