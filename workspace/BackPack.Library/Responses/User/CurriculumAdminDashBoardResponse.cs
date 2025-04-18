using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Dashboard;

namespace BackPack.Library.Responses.User
{
    public class CurriculumAdminDashBoardResponse : ReadBaseResponse
    {
        public CurriculumAdminDashBoardResponseData? Data { get; set; }
    }
    public class CurriculumAdminDashBoardResponseData
    {
        public GetCurriculumAdminDashBoardResult? GetCurriculumAdminDashBoardResults { get; set; }
    }
    public class GetCurriculumAdminDashBoardResult
    {
        public int TotalSubjects { get; set; }
        public int TotalPrivateContent { get; set; }
        public int TotalPublicContent { get; set; }
        public List<ActivityList>? ActivityList { get; set; }
        public List<SubjectList>? SubjectList { get; set; }
    }
    public class SubjectList
    {
        public string? SubjectName { get; set; }
        public int TotalPrivateContent { get; set; }
        public int TotalPublicContent { get; set; }
    }

    public class ActivityList
    {
        public string? Desc { get; set; }
        public int ID { get; set; }
    }
}
 