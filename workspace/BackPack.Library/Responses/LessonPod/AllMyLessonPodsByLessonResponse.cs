using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class AllMyLessonPodsByLessonResponse : ReadBaseResponse
    {
        public AllMyLessonPodsByLessonResult Data { get; set; } = new();
    }

    public class AllMyLessonPodsByLessonResult
    {
        public AllMyLessonPodsByLessonData GetAllMyLessonPodsByLessonResult { get; set; } = new();
    }

    public class AllMyLessonPodsByLessonData
    {
        public List<AllMyLessonPodData> MyLessons { get; set; } = new();
        public List<AllMyLessonPodData> SharedLessons { get; set; } = new();
        public List<AllMyLessonPodCapsuleData> CapsuleLessons { get; set; } = new();
    }

    public class AllMyLessonPodData
    {
        public int LessonUnitID { get; set; }
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ModifiedDate { get; set; } = string.Empty;
    }

    public class AllMyLessonPodCapsuleData : AllMyLessonPodData
    {
        public int PublishCourseCapsuleLessonPodID { get; set; }
        public string CourseCapsuleName { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
    }
}
