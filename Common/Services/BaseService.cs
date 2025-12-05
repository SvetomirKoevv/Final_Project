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

    public List<T> GetAll()
    {
        return itmes.ToList();
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
