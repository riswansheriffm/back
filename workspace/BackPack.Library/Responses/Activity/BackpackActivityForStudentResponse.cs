using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Activity
{
    public class BackpackActivityForStudentResponse : ReadBaseResponse
    {
        public BackpackActivityForStudentDataResult Data { get; set; } = new();
    }

    public class BackpackActivityForStudentDataResult
    {
        public BackpackActivityForStudentData GetBackpackActivityForStudentResult { get; set; } = new();
    }

    public class BackpackActivityForStudentData : BackpackActivityStudentData
    {        
        public List<BackpackActivityForStudentFeedback> Feedback { get; set; } = new();
        public List<BackpackActivityForStudentParentContent> ContainedView { get; set; } = new();
        public List<BackpackActivityStudentData> OtherActivities { get; set; } = [];
    }

    public class BackpackActivityForStudentFeedback
    {
        public int ReworkCount { get; set; }
        public int Grade { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public string FeedbackDate { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string TeacherRecording { get; set; } = string.Empty;
        public string TeacherInk { get; set; } = string.Empty;
        public string ModifiedDate { get; set; } = string.Empty;
    }

    public class BackpackActivityForStudentParentContent
    {
        public string SlideID { get; set; } = string.Empty;
        public string ContentID { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class BackpackActivityStudentData
    {
        public int ContentID { get; set; }
        public int Completed { get; set; }
        public bool IsReadonly { get; set; }
        public bool FollowTheFlow { get; set; }
        public bool AutoHint { get; set; }
        public bool IsCanvas { get; set; }
        public string ActivityJson { get; set; } = string.Empty;
        public string WorkJson { get; set; } = string.Empty;
        public string InkJson { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string ContentMode { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string SpentTime { get; set; } = string.Empty;
        public string ContentName { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
