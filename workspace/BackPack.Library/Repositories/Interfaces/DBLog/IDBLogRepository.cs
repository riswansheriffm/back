
using BackPack.Library.Requests.DBLog;

namespace BackPack.Library.Repositories.Interfaces.DBLog
{
    public interface IDBLogRepository
    {
        Task<bool> CreateRequestAsync(DBLogRequest request);

        Task<bool> UpdateRequestAsync(DBLogRequest request);
    }
}
