using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class AllDistributedLessonpodsByTeacherResponse : ReadBaseResponse
    {
        public AllDistributedLessonpodsByTeacherResult Data { get; set; } = new();
    }

    public class AllDistributedLessonpodsByTeacherResult
    {
        public List<AllDistributedLessonpodsByTeacherData> GetAllDistributedLessonpodsByTeacherResult { get; set; } = [];
    }

    public class AllDistributedLessonpodsByTeacherData
    {
        public int LessonUnitDistID { get; set; }
        public bool TargetdatePassed { get; set; }
        public string LessonUnitName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LessonfolderName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string Targetdate { get; set; } = string.Empty;
        public List<string> StudentNameList { get; set; } = new();
    }

    public class AllDistributedLessonpodsByTeacherQueryResponse : AllDistributedLessonpodsByTeacherData
    {
        public string Students { get; set; } = string.Empty;
    }
}
