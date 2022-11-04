using System.Linq.Expressions;
using Twitter.Entities.Base;

namespace Twitter.Repository;

public interface IRepository<T> where T : IBaseEntity
{
    IQueryable<T> GetAll();
    IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
    T GetById(Guid id);
    T Save(T obj);
    void Delete(T obj);
}