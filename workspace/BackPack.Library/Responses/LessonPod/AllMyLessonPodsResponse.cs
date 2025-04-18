using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class AllMyLessonPodsResponse : ReadBaseResponse
    {
        public AllMyLessonPodsDataResult Data { get; set; } = new();
    }

    public class AllMyLessonPodsDataResult
    {
        public List<AllMyLessonPodsData> GetAllMyLessonUnitByLessonResult { get; set; } = [];
    }

    public class AllMyLessonPodsData
    {
        public int LessonUnitID { get; set; }
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string VersionNumber { get; set; } = string.Empty;
        public string ModifiedDate { get; set; } = string.Empty;
    }
}
