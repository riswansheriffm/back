using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<Tuple<IEnumerable<dynamic>, int>> UserForLogin(LoginRequest request, string userType);

        Task<int> UpdateWrongPassword(LoginRequest request, string userType);

        Task ResetWrongPassword(LoginRequest request, string userType);
    }
}
