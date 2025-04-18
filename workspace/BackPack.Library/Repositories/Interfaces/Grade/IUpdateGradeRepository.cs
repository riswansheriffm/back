using BackPack.Library.Requests.Grade;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.Grade
{
    public interface IUpdateGradeRepository
    {
        Task<BaseResponse> UpdateGradeAsync(UpdateGradeRequest request);
    }
}
