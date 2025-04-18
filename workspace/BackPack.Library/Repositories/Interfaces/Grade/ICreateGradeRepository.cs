using BackPack.Library.Requests.Grade;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.Grade
{
    public interface ICreateGradeRepository
    {
        Task<BaseResponse> CreateGradeAsync(CreateGradeRequest request);
    }
}
