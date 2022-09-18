using Persistence.Models;

namespace Persistence.IRepositories;

public interface IIBANStoreRepository
{
    Task<IBANStore> GetNonActiveIban();

    void UpdateActiveFlag(int id, bool isActive);
}