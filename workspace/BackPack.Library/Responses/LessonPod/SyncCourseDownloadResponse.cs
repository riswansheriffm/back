using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class SyncCourseDownloadResponse : ReadBaseResponse
    {
        public List<SyncCourseDownloadData>? Data { get; set; }
    }

    public class SyncCourseDownloadData
    {
        public int CourseID { get; set; }
        public int LessonUnitCount { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDesc { get; set; }
    }
}
