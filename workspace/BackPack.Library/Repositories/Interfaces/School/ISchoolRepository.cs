using BackPack.Library.Responses.School;

namespace BackPack.Library.Repositories.Interfaces.School
{
    public interface ISchoolRepository
    {
        Task<SchoolResponse> GetSchoolAsync(int SchoolID);

        Task<AllSchoolResponse> GetAllSchoolAsync(int DomainID);
    }
}
