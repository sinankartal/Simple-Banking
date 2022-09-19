using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence.Data;

public class TransactionLimitRepository: ITransactionLimitRepository
{
    #region property

    private readonly ApplicationDbContext _applicationDbContext;
    private DbSet<TransactionLimit> entities;

    #endregion

    #region Constructor

    public TransactionLimitRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        entities = _applicationDbContext.Set<TransactionLimit>();
    }

    #endregion
    public Task<TransactionLimit> GetByTypeAsync(TransactionLimitType type)
    {
        return entities.FirstOrDefaultAsync(s => s.Type.Equals(type));
    }
}