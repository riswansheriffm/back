using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class LessonPodSummaryForAStudentResponse : ReadBaseResponse
    {
        public List<LessonPodSummaryForAStudentData> Data { get; set; } = new();
    }

    public class LessonPodSummaryForAStudentData
    {
        public int ContentID { get; set; }
        public bool ParentAccess { get; set; }
        public string ContentName { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string ActualDateOfCompletion { get; set; } = string.Empty;
        public string ActualTimeOfCompletion { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
        public string ActualCompletionDateTime { get; set; } = string.Empty;
        public string SpentTime { get; set; } = string.Empty;
    }
}
