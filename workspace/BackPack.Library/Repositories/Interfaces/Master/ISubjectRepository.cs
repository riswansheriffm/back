using BackPack.Library.Responses.District;
using BackPack.Library.Responses.Master.Subject;

namespace BackPack.Library.Repositories.Interfaces.Master
{
    public interface ISubjectRepository
    {
        Task<SubjectResponse> GetSubjectAsync(int SubjectID);

        Task<AllSubjectResponse> GetAllSubjectAsync(int DomainID);

        Task<AllSubjectsByTeacherResponse> AllSubjectsByTeacherAsync(int DomainID, int TeacherID);

        Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID);
    }
}
