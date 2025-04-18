using BackPack.Library.Requests.User;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.User;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface IUserRepository
    {
        Task<BaseResponse> ActivateUserAccountAsync(ResetPasswordRequest request, string Password, string PasswordSalt);
        Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request, string Password, string PasswordSalt);

        Task<UserResponse> UserAsync(int LoginID);

        Task<AllUserResponse> AllUsersAsync(int DomainID, int SchoolID, string RoleName);

        Task<AllDomainUsersResponse> AllDomainUserAsync(int DomainID, int SchoolID, string Role);

        Task<AllTeacherByClassResponse> AllTeacherByClassAsync(int LoginID, int CourseID);

        Task<SuperCaResponse> SuperCaAsync(int domainId);
    }
}
