using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IDBLogRepository
    {
        Task<bool> CreateRequestAsync(DBLogRequest request);

        Task<bool> UpdateRequestAsync(DBLogRequest request);
    }
}
