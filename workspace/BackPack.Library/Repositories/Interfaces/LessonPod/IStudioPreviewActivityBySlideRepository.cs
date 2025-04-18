
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IStudioPreviewActivityBySlideRepository
    {
        Task<StudioPreviewActivityBySlideResponse> GetLPStudioPreviewActivityBySlideAsync(int LoginID, int LessonUnitID, string SlideID, string PreviewMode);
    }
}
