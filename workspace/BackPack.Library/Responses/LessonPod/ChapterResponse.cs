using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class ChapterResponse : ReadBaseResponse
    {
        public ChapterResponseData? Data { get; set; }
    }
    public class ChapterResponseData
    {
        public GetChapterResult? GetChapterResult { get; set; }
    }
    public class GetChapterResult
    {
        public int ChapterID { get; set; }
        public string? ChapterName { get; set; }
        public string? ChapterDesc { get; set; }
        public string? ImageURL { get; set; }
        public int ActivityBy { get; set; }
        public int SubjectID { get; set; }
    }
}
