using Data.Entitys;
using Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IBaseService<T> : IBaseRepository<T> where T : EntityBase
    {

        IQueryable<T> GetPaging();
    }


}