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
namespace Core.Service
{
    /// <summary>
    /// Service基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : IBaseService<T> where T : EntityBase
    {
        protected readonly IBaseRepository<T> _repository;
        public BaseService(BaseRepository<T> repository)
        {
            _repository = repository;
        }

        public IQueryable<T> GetPaging()
        {
            throw new NotImplementedException();
        }
    }
}
