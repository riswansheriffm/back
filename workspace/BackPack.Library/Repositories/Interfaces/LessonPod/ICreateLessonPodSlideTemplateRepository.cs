using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ICreateLessonPodSlideTemplateRepository
    {
        Task<CreateLessonPodSlideTemplateResponse> CreateLessonPodSlideTemplateAsync(CreateLessonPodSlideTemplateRequest request);
    }
}
