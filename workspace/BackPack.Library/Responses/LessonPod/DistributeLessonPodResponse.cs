using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class DistributeLessonPodResponse : ReadBaseResponse
    {
        public DistributeLessonPodDataResult Data { get; set; } = new();
    }

    public class DistributeLessonPodDataResult
    {
        public DistributeLessonPodData GetDistributeLessonUnitResult { get; set; } = new();
    }

    public class DistributeLessonPodData
    {
        public int LessonUnitID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public List<LessonPodSlideListResponse>? Slides { get; set; }
    }

    public class DistributeLessonPodQueryResponse
    {
        public int LessonUnitID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string LessonJson { get; set; } = string.Empty;
    }
}
