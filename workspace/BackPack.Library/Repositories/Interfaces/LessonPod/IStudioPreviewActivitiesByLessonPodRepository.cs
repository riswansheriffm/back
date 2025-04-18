
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IStudioPreviewActivitiesByLessonPodRepository
    {
        Task<StudioPreviewActivitiesByLessonPodResponse> GetLPStudioPreviewActivitiesByLessonPodAsync(int LoginID, int LessonUnitID, string PreviewMode);
    }
}
