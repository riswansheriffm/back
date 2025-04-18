using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class LessonFoldersBySubjectResponse : ReadBaseResponse
    {
        public LessonFoldersBySubjectResponseDataResult Data { get; set; } = new();
    }

    public class LessonFoldersBySubjectResponseDataResult
    {
        public List<LessonFoldersBySubjectResponseData> GetBPLessonFoldersBySubjectResult { get; set; } = [];
    }

    public class LessonFoldersBySubjectResponseData
    {
        public int LessonID { get; set; }
        public int LessonUnits { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
    }
}
