using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class AllActivitiesByLessonPodResponse : ReadBaseResponse
    {
        public AllActivitiesByLessonPodResult Data { get; set; } = new();
    }

    public class AllActivitiesByLessonPodResult
    {
        public List<AllActivitiesByLessonPodData> GetAllActivitiesByLessonUnitResult { get; set; } = [];
    }

    public class AllActivitiesByLessonPodData
    {
        public string SlideID { get; set; } = string.Empty;
        public string SlideType { get; set; } = string.Empty;
        public string SlideName { get; set; } = string.Empty;
    }

    public class AllActivitiesByLessonPodQueryData
    {
        public string LessonJson { get; set; } = string.Empty;
    }
}
