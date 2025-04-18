using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Repositories.Interfaces.Master.Subject;
using BackPack.Library.Requests.Master.Subject;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Subject;
using BackPack.Library.Services.Interfaces.Master;
using BackPack.Library.Responses.District;

namespace BackPack.Library.Services.Services.Master
{
    public class SubjectService(
        ISubjectRepository subjectRepository,
        ICreateSubjectRepository createSubjectRepository,
        IUpdateSubjectRepository updateSubjectRepository,
        IDeleteSubjectRepository deleteSubjectRepository
        ) : ISubjectService
    {
        #region GetSubjectAsync
        public async Task<SubjectResponse> GetSubjectAsync(int SubjectID)
        {
            SubjectResponse response = await subjectRepository.GetSubjectAsync(SubjectID);

            return response;
        }
        #endregion

        #region GetAllSubjectAsync
        public async Task<AllSubjectResponse> GetAllSubjectAsync(int DomainID)
        {
            AllSubjectResponse response = await subjectRepository.GetAllSubjectAsync(DomainID);

            return response;
        }
        #endregion

        #region AllSubjectsByTeacherAsync
        public async Task<AllSubjectsByTeacherResponse> AllSubjectsByTeacherAsync(int DomainID, int TeacherID)
        {
            AllSubjectsByTeacherResponse response = await subjectRepository.AllSubjectsByTeacherAsync(DomainID, TeacherID);

            return response;
        }
        #endregion

        #region CreateSubjectAsync
        public async Task<BaseResponse> CreateSubjectAsync(CreateSubjectRequest request)
        {
            BaseResponse response = await createSubjectRepository.CreateSubjectAsync(request);

            return response;
        }
        #endregion

        #region UpdateSubjectAsync
        public async Task<BaseResponse> UpdateSubjectAsync(UpdateSubjectRequest request)
        {
            BaseResponse response = await updateSubjectRepository.UpdateSubjectAsync(request);

            return response;
        }
        #endregion

        #region DeleteSubjectAsync
        public async Task<BaseResponse> DeleteSubjectAsync(DeleteSubjectRequest request)
        {
            BaseResponse response = await deleteSubjectRepository.DeleteSubjectAsync(request);

            return response;
        }
        #endregion

        #region GetAllSubjectsByDomainAsync
        public async Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID)
        {
            AllSubjectsByDomainResponse response = await subjectRepository.GetAllSubjectsByDomainAsync(DomainID);

            return response;
        }
        #endregion
    }
}
