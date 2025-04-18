using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class PendingLessonPodsForAStudentResponse : ReadBaseResponse
    {
        public PendingLessonUnitsForAStudentDataResult Data { get; set; } = new();
    }

    public class PendingLessonUnitsForAStudentDataResult
    {
        public List<PendingLessonUnitsForAStudentData> GetPendingLessonUnitsForAStudentResult { get; set; } = [];
    }

    public class PendingLessonUnitsForAStudentData
    {
        public int LessonUnitDistID { get; set; }
        public int Offline { get; set; }
        public float CompletedPercent { get; set; }
        public bool ParentAccess { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string SpentTime { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
    }
}
