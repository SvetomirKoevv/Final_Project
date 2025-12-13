using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Common.Entities;
using Common.Persistance;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<T>> GetAll()
    {
        return await itmes.ToListAsync();
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

    public async Task Create(T item)
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
