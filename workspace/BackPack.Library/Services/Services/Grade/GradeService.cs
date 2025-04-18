using BackPack.Dependency.Library.Responses;
using BackPack.Library.Repositories.Interfaces.Grade;
using BackPack.Library.Requests.Grade;
using BackPack.Library.Responses.Grade;
using BackPack.Library.Services.Interfaces.Grade;

namespace BackPack.Library.Services.Services.Grade
{
    public class GradeService(
        IGradeRepository gradeRepository,
        ICreateGradeRepository createGradeRepository,
        IUpdateGradeRepository updateGradeRepository,
        IDeleteGradeRepository deleteGradeRepository
        ) : IGradeService  
    {
        #region GetGradeAsync
        public async Task<GradeResponse> GetGradeAsync(int GradeID)
        {
            GradeResponse response = await gradeRepository.GetGradeAsync(GradeID);

            return response;
        }
        #endregion

        #region GetAllGradeAsync
        public async Task<AllGradeResponse> GetAllGradeAsync(int DomainID)
        {
            AllGradeResponse response = await gradeRepository.GetAllGradeAsync(DomainID);

            return response;
        }
        #endregion

        #region CreateGradeAsync
        public async Task<BaseResponse> CreateGradeAsync(CreateGradeRequest request)
        {
            BaseResponse response = await createGradeRepository.CreateGradeAsync(request);

            return response;
        }
        #endregion

        #region UpdateGradeAsync
        public async Task<BaseResponse> UpdateGradeAsync(UpdateGradeRequest request)
        {
            BaseResponse response = await updateGradeRepository.UpdateGradeAsync(request);

            return response;
        }
        #endregion

        #region DeleteGradeAsync
        public async Task<BaseResponse> DeleteGradeAsync(DeleteGradeRequest request)
        {
            BaseResponse response = await deleteGradeRepository.DeleteGradeAsync(request);

            return response;
        }
        #endregion
    }
} 
