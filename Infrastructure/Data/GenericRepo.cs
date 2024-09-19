using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepo<T>(StoreContext storeContext) : IGenericRepo<T> where T: BaseEntity
{
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await storeContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await storeContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Add(T entity)
    {
        storeContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        storeContext.Set<T>().Attach(entity);
        storeContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        storeContext.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await storeContext.SaveChangesAsync() > 0;
    }

    public bool IsExists(int id)
    {
        return storeContext.Set<T>().Any(x => x.Id == id);
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(storeContext.Set<T>().AsQueryable(), spec);
    }
}