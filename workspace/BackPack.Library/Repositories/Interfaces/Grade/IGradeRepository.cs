using BackPack.Library.Responses.Grade;

namespace BackPack.Library.Repositories.Interfaces.Grade
{
    public interface IGradeRepository
    {
        Task<GradeResponse> GetGradeAsync(int GradeID);

        Task<AllGradeResponse> GetAllGradeAsync(int DomainID);
    }
}
