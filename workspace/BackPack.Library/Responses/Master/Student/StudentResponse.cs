using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Student
{
    public class StudentResponse : ReadBaseResponse
    {
        public StudentResponseData? Data { get; set; }
    }
    public class StudentResponseData
    {
        public GetStudentResult? GetStudentResult { get; set; }
    }
    public class GetStudentResult
    {
        public string? FName { get; set; } = string.Empty;
        public string? LName { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? LoginName { get; set; } = string.Empty;
        public string? GmailID { get; set; } = string.Empty;
    }
}
