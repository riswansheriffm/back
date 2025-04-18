
using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{
    public class AllTeacherByClassResponseEvent : ReadBaseResponse
    {
        public AllTeacherByClassResponseEventData Data { get; set; } = new();
    }

    public class AllTeacherByClassResponseEventData
    {
        public List<AllTeacherByClassResponseEventResult> GetAllTeachersByClassResult { get; set; } = [];
    }

    public class AllTeacherByClassResponseEventResult
    {
        public int CourseID { get; set; }
        public int TeacherID { get; set; }
        public string LoginName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
