using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Activity
{
    public class UpcomingAssignmentsResponse : ReadBaseResponse
    {
        public UpcomingAssignmentsDataResponseResult? Data { get; set; }
    }

    public class UpcomingAssignmentsDataResponseResult
    {
        public List<UpcomingAssignmentsDataResponse> GetBPUpcomingAssignmentsResult { get; set; } = [];
    }

    public class UpcomingAssignmentsDataResponse
    {
        public int LessonUnitDistID { get; set; }
        public bool ParentAccess { get; set; }
        public string? CourseName { get; set; }
        public string? LessonName { get; set; }
        public string? LessonDesc { get; set; }
        public string? Status { get; set; }
        public string? TargetDateOfCompletion { get; set; }
        public string? TargetTimeOfCompletion { get; set; }
        public string? TargetCompletionDateTime { get; set; }
    }
}
