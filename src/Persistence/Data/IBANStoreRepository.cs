using Microsoft.EntityFrameworkCore;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence.Data;

public class IBANStoreRepository: IIBANStoreRepository
{
    #region property

    private readonly ApplicationDbContext _applicationDbContext;
    private DbSet<IBANStore> entities;

    #endregion

    #region Constructor

    public IBANStoreRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        entities = _applicationDbContext.Set<IBANStore>();
    }

    #endregion

    public async virtual Task<IBANStore> GetNonActiveIban()
    {
        return await entities.FirstOrDefaultAsync(s => !s.IsActive);
    }

    public async void UpdateActiveFlag(int id, bool isActive)
    {
        IBANStore iban = await entities.FindAsync(id);
        iban.IsActive = isActive;
        entities.Update(iban);
        await _applicationDbContext.SaveChangesAsync();
    }
}