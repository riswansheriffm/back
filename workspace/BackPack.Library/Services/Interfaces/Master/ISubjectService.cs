using BackPack.Library.Requests.Master.Subject;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Subject;
using BackPack.Library.Responses.District;

namespace BackPack.Library.Services.Interfaces.Master
{
    public interface ISubjectService
    {
        Task<SubjectResponse> GetSubjectAsync(int SubjectID);

        Task<AllSubjectResponse> GetAllSubjectAsync(int DomainID);

        Task<AllSubjectsByTeacherResponse> AllSubjectsByTeacherAsync(int DomainID, int TeacherID);

        Task<BaseResponse> CreateSubjectAsync(CreateSubjectRequest request);

        Task<BaseResponse> UpdateSubjectAsync(UpdateSubjectRequest request);

        Task<BaseResponse> DeleteSubjectAsync(DeleteSubjectRequest request);

        Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID);
    }
}
