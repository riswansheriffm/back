using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class DistributedLessonPodResponse : ReadBaseResponse
    {
        public DistributedLessonPodData Data { get; set; } = new();
    }

    public class DistributedLessonPodData
    {
        public int LessonUnitDistID { get; set; }
        public object CourseID { get; set; } = new();
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string WhomToDistribute { get; set; } = string.Empty;
        public List<int> StudentIDs { get; set; } = new();
        public List<string> GroupIDs { get; set; } = new();
        public List<LessonPodSlideListResponse> Slides { get; set; } = new();
    }

    public class DistributedLessonPodQueryResponse
    {
        public int LessonUnitDistID { get; set; }
        public object CourseID { get; set; } = new();
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string WhomToDistribute { get; set; } = string.Empty;
        public string GroupIDs { get; set; } = string.Empty;
        public string LessonJson { get; set; } = string.Empty;
    }
}
