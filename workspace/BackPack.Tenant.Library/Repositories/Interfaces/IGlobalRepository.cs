
namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGlobalRepository
    {
        Task<string> GetLoginNameByID(int userID, string userType);
    }
}
