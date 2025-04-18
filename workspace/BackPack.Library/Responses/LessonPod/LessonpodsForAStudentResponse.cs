
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class LessonpodsForAStudentResponse : ReadBaseResponse
    {
        public LessonpodsForAStudentResult Data { get; set; } = new();
    }

    public class LessonpodsForAStudentResult
    {
        public List<CompletedLessonPodsByLessonData> Completed { get; set; } = [];

        public List<PendingLessonUnitsForAStudentData> Pending { get; set; } = [];
    }
}
