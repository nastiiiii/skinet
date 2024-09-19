using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepo<T> where T: BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    
    Task<bool> SaveChangesAsync();
    bool IsExists(int id);
}