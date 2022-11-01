using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Twitter.Context.Context;
using Twitter.Entities.Base;

namespace Twitter.Repository;

public class Repository<T> : IRepository<T> where T : class, IBaseEntity 
{
    private readonly MainDbContext context;

    public Repository(MainDbContext context)
    {
        this.context = context;
    }


    public IQueryable<T> GetAll()
    {
        return context.Set<T>().AsQueryable();
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
    {
        return context.Set<T>().Where(predicate).AsQueryable();
    }

    public T GetById(Guid id)
    {
        return context.Set<T>().FirstOrDefault(x => x.Id == id) ?? throw new NullReferenceException(); //TODO обработать
    }

    public T Save(T obj)
    {

        try
        {
            if (obj.IsNew)
            {
                obj.Init();
                var result = context.Set<T>().Add(obj);
                context.SaveChanges();
                return result.Entity;
            }
            else
            {
                obj.ModificationTime = DateTime.UtcNow;
                var result = context.Set<T>().Attach(obj);
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();
                return result.Entity;
            }
        }
        catch (Exception e)
        {
            //TODO logger
            throw e;
        }
    }

    public void Delete(T obj)
    {
        context.Remove(obj);
        context.SaveChanges();
    }
}