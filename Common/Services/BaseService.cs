using System.Linq.Expressions;
using Common.Entities;
using Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class BaseService<T>
where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> itmes;

    public BaseService()
    {
        _context = new AppDbContext();
        itmes = _context.Set<T>();
    }

    public List<T> GetAll(
        Expression<Func<T, bool>> filter,
        string sortProperty,
        bool sortAscending,
        int pageNumber,
        int pageSize
    )
    {
        IQueryable<T> query = itmes.Where(filter);

        if (sortAscending)
        {
            query = query.OrderBy(e => EF.Property<object>(e, sortProperty));
        }
        else
        {
            query = query.OrderByDescending(e => EF.Property<object>(e, sortProperty));
        }

        var result = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        return result;
    }

    public T GetById(int id)
    {
        return itmes.FirstOrDefault(i => i.Id == id);
    }

    public void Create(T item)
    {
        itmes.Add(item);
        _context.SaveChanges();
    }

    public void Update(T item)
    {
        itmes.Update(item);
        _context.SaveChanges();
    }

    public T Delete(int id)
    {
        T item = GetById(id);
        if (item != null)
        {
            itmes.Remove(item);
            _context.SaveChanges();
        }
        return item;
    }
}
