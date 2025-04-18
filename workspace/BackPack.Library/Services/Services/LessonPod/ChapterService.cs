using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Services.Interfaces.LessonPod;

namespace BackPack.Library.Services.Services.LessonPod
{
    public class ChapterService(
        IChapterBySubjectRepository chapterBySubjectRepository,
        ICreateChapterRepository createChapterRepository,
        IUpdateChapterRepository updateChapterRepository,
        IDeleteChapterRepository deleteChapterRepository,
        IChapterRepository chapterRepository
        ) : IChapterService
    {
        #region AllChaptersBySubjectAsync
        public async Task<AllChaptersBySubjectResponse> AllChaptersBySubjectAsync(int SubjectID)
        {
            AllChaptersBySubjectResponse response = await chapterBySubjectRepository.AllChaptersBySubjectAsync(SubjectID);

            return response;
        }
        #endregion

        #region CreateChapterAsync
        public async Task<BaseResponse> CreateChapterAsync(CreateChapterRequest request)
        {
            BaseResponse response = await createChapterRepository.CreateChapterAsync(request);

            return response;
        }
        #endregion

        #region UpdateChapterAsync
        public async Task<BaseResponse> UpdateChapterAsync(UpdateChapterRequest request)
        {
            BaseResponse response = await updateChapterRepository.UpdateChapterAsync(request);

            return response;
        }
        #endregion

        #region DeleteChapterAsync
        public async Task<BaseResponse> DeleteChapterAsync(DeleteChapterRequest request)
        {
            BaseResponse response = await deleteChapterRepository.DeleteChapterAsync(request);

            return response;
        }
        #endregion

        #region ChaptersAsync
        public async Task<ChapterResponse> ChaptersAsync(int ChapterID)
        {
            ChapterResponse response = await chapterRepository.ChaptersAsync(ChapterID);

            return response;
        }
        #endregion
    }
}
