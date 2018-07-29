using Core.IServices;
using Data.Entitys;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    /// <summary>
    /// Service基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : BaseRepository<T>, IBaseService<T> where T : EntityBase
    {
        public BaseService(DbContext context) : base(context)
        {
        }

        public IQueryable<T> GetPaging()
        {
            throw new NotImplementedException();
        }
    }
}
