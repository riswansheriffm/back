using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ICreateLessonPodRepository
    {
        Task<CreateLessonPodResponse> CreateLessonPodAsync(CreateLessonPodRequest request);
    }
}
