using System.Linq.Expressions;
using System.Reflection;
using Common.Entities;
using Common.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Common.Services;

public class BaseService<T>
where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> itmes;

    public BaseService()
    {
        _context = new AppDbContext();
        itmes = _context.Set<T>();
    }

    public async Task<List<T>> GetAll(params string[] includes)
    {
        IQueryable<T> result = itmes;
        Type type = typeof(T);
        List<PropertyInfo> properties = type.GetProperties().ToList();

        foreach (var include in includes)
        {
            PropertyInfo prop = properties
                .FirstOrDefault(p => p.Name == include);

            TypeInfo propertyType = prop.GetType().GetTypeInfo();
            if (prop != null)
            {
                result = result.Include(x => EF.Property<T>(x, prop.Name));
            }
        }
        List<T> res = await result.ToListAsync();

        return await result.ToListAsync();
    }
    public async Task<List<T>> GetAllFiltered(
        Expression<Func<T, bool>> filter = null,
        string sortProperty = null,
        bool sortAscending = true,
        int pageNumber = 1,
        int pageSize = 10
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

        var result = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return result;
    }

    public async Task<T> GetById(int id)
    {
        return await itmes.FirstOrDefaultAsync(i => i.Id == id);
    }

    public virtual async Task Create(T item)
    {
        itmes.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T item)
    {
        itmes.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<T> Delete(int id)
    {
        T item = await GetById(id);
        if (item != null)
        {
            itmes.Remove(item);
            await _context.SaveChangesAsync();
        }
        return item;
    }
}
