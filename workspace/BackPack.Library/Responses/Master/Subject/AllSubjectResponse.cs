using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Subject
{
    public class AllSubjectResponse : ReadBaseResponse
    {
        public AllSubjectResponseData? Data { get; set; }
    }
    public class AllSubjectResponseData
    {
        public List<GetAllSubjectResult>? GetAllSubjectResult { get; set; }
    }
    public class GetAllSubjectResult
    {
        public string? SubjectDesc { get; set; }
        public string? SubjectName { get; set; }
        public int SubjectID { get; set; }
        public int GradeID { get; set; }
    }
}
