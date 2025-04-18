using BackPack.Library.Requests.Grade;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.Grade
{
    public interface IDeleteGradeRepository
    {
        Task<BaseResponse> DeleteGradeAsync(DeleteGradeRequest request);
    }
}
