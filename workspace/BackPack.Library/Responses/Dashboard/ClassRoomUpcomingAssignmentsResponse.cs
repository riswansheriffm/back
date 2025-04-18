using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Dashboard
{
    public class ClassRoomUpcomingAssignmentsResponse : ReadBaseResponse
    {
        public ClassRoomUpcomingAssignmentsDataResult Data { get; set; } = new();
    }

    public class ClassRoomUpcomingAssignmentsDataResult
    {
        public List<ClassRoomUpcomingAssignmentsData> GetCRUpcomingAssignmentsResult {  get; set; } = [];
    }

    public class ClassRoomUpcomingAssignmentsData
    {
        public int LessonUnitDistID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
    }
}
