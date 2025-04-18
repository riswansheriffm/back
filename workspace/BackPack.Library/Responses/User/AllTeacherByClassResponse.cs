using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class AllTeacherByClassResponse : ReadBaseResponse
    {
        public AllTeacherByClassResponseData? Data { get; set; }
    }
    public class AllTeacherByClassResponseData
    {
        public List<GetAllTeachersByClassResult>? GetAllTeachersByClassResult { get; set; }
    }
    public class GetAllTeachersByClassResult
    {
        public int CourseID { get; set; }
        public int TeacherID { get; set; }
        public string? LoginName { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
    }
}
