
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Repositories;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Repositories.Repositories.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Services.Interfaces.User;

namespace BackPack.Library.Services.Services.User
{
    public class DeleteUserService(
        IDeleteStudentRepository deleteStudentRepository
        ) : GenericRepository, IDeleteUserService
    {
        #region DeleteStudentAsync
        public async Task<BaseResponse> DeleteStudentAsync(DeleteStudentRequest request)
        {
            BaseResponse response = await deleteStudentRepository.DeleteStudentAsync(request);

            return response;
        }
        #endregion
    }
}
