using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IAllSlideTemplateRepository
    {
        Task<AllSlideTemplateResponse> AllSlideTemplateAsync(int DomainID, int LoginID);
    }
}
