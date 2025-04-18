
using BackPack.Library.Requests.DBLog;

namespace BackPack.Library.Services.Interfaces.DBLog
{
    public interface IDBLogService
    {
        Task<bool> CreateRequestAsync(DBLogRequest request);

        Task<bool> UpdateRequestAsync(DBLogRequest request);
    }
}
