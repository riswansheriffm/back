using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Services.Interfaces.LessonPod
{
    public interface IChapterService
    {
        Task<AllChaptersBySubjectResponse> AllChaptersBySubjectAsync(int SubjectID);

        Task<BaseResponse> CreateChapterAsync(CreateChapterRequest request);

        Task<BaseResponse> UpdateChapterAsync(UpdateChapterRequest request);

        Task<BaseResponse> DeleteChapterAsync(DeleteChapterRequest request);

        Task<ChapterResponse> ChaptersAsync(int ChapterID);
    }
}
