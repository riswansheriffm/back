using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IAllDistributedLessonpodsByTeacherRepository
    {
        Task<AllDistributedLessonpodsByTeacherResponse> AllDistributedLessonpodsByTeacherAsync(int LoginID, int LessonUnitID);
    }
}
