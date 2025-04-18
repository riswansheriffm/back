using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class SyncCourseLessonUnitDownloadResponse : ReadBaseResponse
    {
        public List<SyncCourseLessonUnitDownloadData>? Data { get; set; }
    }

    public class SyncCourseLessonUnitDownloadData
    {
        public int LessonUnitDistID { get; set; }
        public int DataSize { get; set; }
    }
}
