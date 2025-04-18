using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class CompletedLessonPodsByLessonResponse : ReadBaseResponse
    {
        public CompletedLessonPodsByLessonDataResult Data { get; set; } = new();
    }

    public class CompletedLessonPodsByLessonDataResult
    {
        public List<CompletedLessonPodsByLessonData> GetBPCompletedLessonUnitsByLessonResult { get; set; } = [];
    }

    public class CompletedLessonPodsByLessonData
    {
        public int LessonUnitDistID { get; set; }
        public bool ParentAccess { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string SpentTime { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
    }
}
