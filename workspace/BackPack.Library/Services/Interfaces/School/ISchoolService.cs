using BackPack.Library.Requests.School;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.School;

namespace BackPack.Library.Services.Interfaces.School
{
    public interface ISchoolService
    {
        Task<SchoolResponse> GetSchoolAsync(int SchoolID);

        Task<AllSchoolResponse> GetAllSchoolAsync(int DomainID);

        Task<CreateSchoolResponse> CreateSchoolAsync(CreateSchoolRequest request);

        Task<BaseResponse> UpdateSchoolAsync(UpdateSchoolRequest request);

        Task<BaseResponse> DeleteSchoolAsync(DeleteSchoolRequest request);

        Task<BaseResponse> CreateSchoolBulkAsync(CreateSchoolBulkRequest request);
    }
}
