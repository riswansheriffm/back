using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Subject
{
    public class SubjectResponse : ReadBaseResponse
    {
        public SubjectResponseData? Data { get; set; }
    }
    public class SubjectResponseData
    {
        public GetSubjectResult? GetSubjectResult { get; set; }
    }
    public class GetSubjectResult
    {
        public string? SubjectDesc { get; set; }
        public string? SubjectName { get; set; }
        public int SubjectID { get; set; }
        public int GradeID { get; set; }
    }
}
