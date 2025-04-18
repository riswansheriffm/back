using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Services.Interfaces;

namespace BackPack.Tenant.Library.Services.Services
{
    public class DBLogService(IDBLogRepository logRepository) : IDBLogService
    {
        #region CreateRequestAsync
        public Task<bool> CreateRequestAsync(DBLogRequest request)
        {
            return logRepository.CreateRequestAsync(request);
        }
        #endregion

        #region UpdateRequestAsync
        public Task<bool> UpdateRequestAsync(DBLogRequest request)
        {
            return (logRepository.UpdateRequestAsync(request));
        }
        #endregion
    }
}
