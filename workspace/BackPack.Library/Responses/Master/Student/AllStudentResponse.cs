using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Student
{
    public class AllStudentResponse : ReadBaseResponse
    {
        public AllStudentResponseData Data { get; set; } = new();
    }
    public class AllStudentResponseData
    {
        public List<GetAllStudentsResult> GetAllStudentsResult { get; set; } = [];
    }
    public class GetAllStudentsResult
    {
        public string? FName { get; set; } = string.Empty;
        public string? LName { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? LoginName { get; set; } = string.Empty;
        public int LoginID { get; set; } = 0;
    }
}
