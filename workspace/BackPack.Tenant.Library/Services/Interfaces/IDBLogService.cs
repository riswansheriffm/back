using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface IDBLogService
    {
        Task<bool> CreateRequestAsync(DBLogRequest request);

        Task<bool> UpdateRequestAsync(DBLogRequest request);
    }
}
