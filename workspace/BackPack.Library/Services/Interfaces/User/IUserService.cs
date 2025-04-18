using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface IUserService
    {
        Task<BaseResponse> ActivateUserAccountAsync(ResetPasswordRequest request);

        Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request);       

        Task<UserResponse> UserAsync(int LoginID);

        Task<AllUserResponse> AllUsersAsync(int DomainID, int SchoolID, string RoleName);

        Task<AllDomainUsersResponse> AllDomainUserAsync(int DomainID, int SchoolID, string Role);

        Task<AllTeacherByClassResponse> AllTeacherByClassAsync(int LoginID, int CourseID);

        Task<SuperCaResponse> SuperCaAsync(int domainId);
    }
}
