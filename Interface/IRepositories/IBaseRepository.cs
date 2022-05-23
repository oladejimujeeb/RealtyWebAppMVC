using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.Identity;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IBaseRepository<T> where T:BaseEntity,new()
    {
        Task<T> Get(int id);
        Task<T> Get(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> expression);

        Task<IList<T>> GetList(IList<int> ids);
        Task<bool> Exists(Expression<Func<User, bool>> expression);

        IQueryable<T> Query();

        Task<T> Add(T entity);

        Task<T> Update(T entity);

        Task<bool> Delete(T entity);
        Task<int> SaveChanges();
        IQueryable<T> QueryWhere(Expression<Func<T, bool>> expression);
    }
}