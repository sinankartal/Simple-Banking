using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity
{
    #region property

    private readonly ApplicationDbContext _applicationDbContext;
    private DbSet<T> entities;

    #endregion

    #region Constructor

    protected Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        entities = _applicationDbContext.Set<T>();
    }

    #endregion

    public virtual async Task<T> FindAsync(string id)
    {
        return await entities.FindAsync(id);
    }

    public virtual void SaveAsync()
    {
        _applicationDbContext.SaveChangesAsync();
    }

    public virtual async Task<string> InsertAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }

        entity.Id = Guid.NewGuid().ToString();
        entity.CreateDate = DateTime.Now;
        entity.ModifyDate = DateTime.Now;
        await entities.AddAsync(entity);
        await _applicationDbContext.SaveChangesAsync();
        return entity.Id;
    }

    public virtual void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }
        entity.ModifyDate = DateTime.Now;
        entities.Update(entity);
        _applicationDbContext.SaveChanges();
    }
}