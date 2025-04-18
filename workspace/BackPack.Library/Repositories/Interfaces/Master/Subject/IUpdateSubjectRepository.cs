using BackPack.Library.Requests.Master.Subject;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.Master.Subject
{
    public interface IUpdateSubjectRepository
    {
        Task<BaseResponse> UpdateSubjectAsync(UpdateSubjectRequest request);
    }
}
