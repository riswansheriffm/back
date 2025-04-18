using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class LessonPodResponse : ReadBaseResponse
    {
        public LessonPodResult Data { get; set; } = new();
    }

    public class LessonPodResult
    {
        public LessonPodData GetLessonUnitResult { get; set; } = new();
    }

    public class LessonPodData
    {
        public int LessonUnitID { get; set; }
        public int LessonID { get; set; }
        public int AuthorID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string LessonJson { get; set; } = string.Empty;
    }
}
