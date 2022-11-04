using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Twitter.Context.Context;
using Twitter.Entities.Base;

namespace Twitter.Repository;

public class Repository<T> : IRepository<T> where T : class, IBaseEntity
{
    private readonly MainDbContext _context;

    public Repository(MainDbContext context)
    {
        _context = context;
    }


    public IQueryable<T> GetAll()
    {
        try
        {
            return _context.Set<T>().AsQueryable();
        }
        catch (Exception ex)
        {
            throw new ProcessException("Error while get data from database.", ex);
        }
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }
        catch (Exception ex)
        {
            throw new ProcessException("Error while get data from database.", ex);
        }
    }

    public T GetById(Guid id)
    {
        var model = _context.Set<T>().Find(id);
        ProcessException.ThrowIf(() => model is null, "The entity with this Id was not found at database.");
        return model;
    }

    public T Save(T obj)
    {
        try
        {
            if (obj.IsNew)
            {
                obj.Init();
                var result = _context.Set<T>().Add(obj);
                _context.SaveChanges();
                return result.Entity;
            }
            else
            {
                obj.ModificationTime = DateTime.UtcNow;
                var result = _context.Set<T>().Attach(obj);
                _context.Entry(obj).State = EntityState.Modified;
                _context.SaveChanges();
                return result.Entity;
            }
        }
        catch (Exception e)
        {
            throw new ProcessException("Error while saving entity", e);
        }
    }

    public void Delete(T obj)
    {
        try
        {
            _context.Set<T>().Attach(obj);
            _context.Entry(obj).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new ProcessException("Error while delete entity.", ex);
        }
    }
}