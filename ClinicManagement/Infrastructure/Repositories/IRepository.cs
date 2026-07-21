namespace Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task CreateAsync(T entity);
    
    void Delete(T entity);
}