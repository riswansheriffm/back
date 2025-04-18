using BackPack.Library.Requests.User;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface ILoginRepository
    {
        Task<Tuple<IEnumerable<dynamic>, int>> UserForLogin(LoginRequest request, string userType);

        Task<int> UpdateWrongPassword(LoginRequest request, string userType);

        Task ResetWrongPassword(LoginRequest request, string userType);
    }
}
