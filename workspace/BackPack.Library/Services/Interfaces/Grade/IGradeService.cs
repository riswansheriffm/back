using BackPack.Library.Requests.Grade;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Grade;

namespace BackPack.Library.Services.Interfaces.Grade
{
    public interface IGradeService
    {
        Task<GradeResponse> GetGradeAsync(int GradeID);

        Task<AllGradeResponse> GetAllGradeAsync(int DomainID);

        Task<BaseResponse> CreateGradeAsync(CreateGradeRequest request);

        Task<BaseResponse> UpdateGradeAsync(UpdateGradeRequest request);

        Task<BaseResponse> DeleteGradeAsync(DeleteGradeRequest request);
    }
}
