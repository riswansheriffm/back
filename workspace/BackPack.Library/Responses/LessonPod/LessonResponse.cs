using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class LessonResponse : ReadBaseResponse
    {
        public LessonResponseData? Data { get; set; }
    }
    public class LessonResponseData
    {
        public GetLessonResult? GetLessonResult { get; set; }
    }
    public class GetLessonResult
    {
        public int LessonID { get; set; }
        public int SubjectID { get; set; }
        public string? LessonName { get; set; } = string.Empty;
        public string? LessonDesc { get; set; } = string.Empty;
        public string? ImageURL { get; set; } = string.Empty;
        public int ChapterID { get; set; }
        public string? ChapterName { get; set; } = string.Empty;
        public List<Tags>? Tags { get; set; }
    }
    public class Tags
    {
        public int TagID { get; set; }
        public string? TagName { get; set; } = string.Empty;
    }
}
