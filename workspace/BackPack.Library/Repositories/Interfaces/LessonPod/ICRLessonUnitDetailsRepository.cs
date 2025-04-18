using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ICRLessonUnitDetailsRepository
    {
        Task<CRLessonUnitDetailsResponse> CRLessonUnitDetailsAsync(int LessonUnitDistID);
    }
}
