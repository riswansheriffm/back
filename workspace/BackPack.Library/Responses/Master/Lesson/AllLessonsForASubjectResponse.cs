using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Lesson
{
    public class AllLessonsForASubjectResponse : ReadBaseResponse
    {
        public AllLessonsForASubjectDataResult Data { get; set; } = new();
    }

    public class AllLessonsForASubjectDataResult
    {
        public List<AllLessonsForASubjectData> GetAllLessonsForASubjectResult { get; set; } = [];
    }

    public class AllLessonsForASubjectData
    {
        public int LessonID { get; set; }
        public int SubjectID { get; set; }
        public int DisplayOrder { get; set; }
        public int ChapterID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string ChapterName { get; set; } = string.Empty;
    }
}
