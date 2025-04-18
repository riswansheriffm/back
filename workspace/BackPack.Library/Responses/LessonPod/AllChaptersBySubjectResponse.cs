using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class AllChaptersBySubjectResponse : ReadBaseResponse
    {
        public AllChaptersBySubjectDataResult Data { get; set; } = new();
    }

    public class AllChaptersBySubjectDataResult
    {
        public List<AllChaptersBySubjectData> GetAllChaptersBySubjectResult { get; set; } = [];
    }

    public class AllChaptersBySubjectData
    {
        public int ChapterID { get; set; }
        public string ChapterName { get; set; } = string.Empty;
        public string ChapterDesc { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
    }
}
