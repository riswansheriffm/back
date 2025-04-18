using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Subject
{
    public class AllSubjectsByTeacherResponse : ReadBaseResponse

    {
        public AllSubjectsByTeacherDataResult Data { get; set; } = new();
    }

    public class AllSubjectsByTeacherDataResult
    {
        public List<AllSubjectsByTeacherData> GetAllSubjectsByTeacherResult { get; set; } = new();
    }

    public class AllSubjectsByTeacherData
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }
}
