
using BackPack.Library.Repositories.Interfaces.DBLog;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Services.Interfaces.DBLog;

namespace BackPack.Library.Services.Services.DBLog
{
    public class DBLogService(
        IDBLogRepository dbLogRepository
        ) : IDBLogService
    {
        #region CreateRequestAsync
        public Task<bool> CreateRequestAsync(DBLogRequest request)
        {
            return dbLogRepository.CreateRequestAsync(request);
        }
        #endregion

        #region UpdateRequestAsync
        public Task<bool> UpdateRequestAsync(DBLogRequest request)
        {
            return (dbLogRepository.UpdateRequestAsync(request));
        }
        #endregion
    }
}
