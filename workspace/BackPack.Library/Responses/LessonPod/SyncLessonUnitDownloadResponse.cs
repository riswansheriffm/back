using BackPack.Library.Responses.Activity;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class SyncLessonUnitDownloadResponse : ReadBaseResponse
    {
        public SyncLessonUnitDownloadData? Data { get; set; }
    }

    public class SyncLessonUnitDownloadData
    {
        public int LessonUnitDistID { get; set; }
        public int CourseID { get; set; }
        public int DownloadVersion { get; set; }
        public string? LessonName { get; set; }
        public string? LessonDesc { get; set; }
        public string? Status { get; set; }
        public string? TargetCompletionDateTime { get; set; }
        public List<SyncLessonUnitDownloadActivityData>? Activities { get; set; }
    }

    public class SyncLessonUnitDownloadActivityData
    {
        public int ContentID { get; set; }
        public int Completed { get; set; }
        public bool IsReadonly { get; set; }
        public string? ActivityJson { get; set; }
        public string? WorkJson { get; set; }
        public string? InkJson { get; set; }
        public string? ActivityType { get; set; }
        public string? ContentMode { get; set; }
        public string? TargetDateOfCompletion { get; set; }
        public string? TargetTimeOfCompletion { get; set; }
        public string? SpentTime { get; set; }
        public string? ContentName { get; set; }
        public string? AppName { get; set; }
        public string? TargetCompletionDateTime { get; set; }
        public string? Status { get; set; }
        public List<BackpackActivityForStudentFeedback>? Feedback { get; set; }
    }

    public class SyncLessonUnitDownloadQueryResponse
    {
        public int ContentID { get; set; }
        public int Completed { get; set; }
        public int ReworkCount { get; set; }
        public float TotalPoints { get; set; }
        public bool IsReadonly { get; set; }
        public string? ActivityJson { get; set; }
        public string? WorkJson { get; set; }
        public string? InkJson { get; set; }
        public string? ContentName { get; set; }
        public string? AppName { get; set; }
        public string? ActivityType { get; set; }
        public string? ContentMode { get; set; }
        public string? SpentTime { get; set; }
        public string? TargetDateOfCompletion { get; set; }
        public string? TargetTimeOfCompletion { get; set; }
        public string? TargetCompletionDateTime { get; set; }
        public string? Status { get; set; }
        public string? Feedback { get; set; }
        public int Grade { get; set; }
        public string? FeedbackDate { get; set; }
        public string? Remarks { get; set; }
        public string? ModifiedDate { get; set; }
    }
}
