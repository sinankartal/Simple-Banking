
namespace Persistence.IRepositories;

public interface IRepository<T>
{
    public Task<string> InsertAsync(T entity);
    
    Task<T> FindAsync(string id);

    void SaveAsync();

    void Update(T item);
}