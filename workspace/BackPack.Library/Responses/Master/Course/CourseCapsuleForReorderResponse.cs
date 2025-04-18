using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class CourseCapsuleForReorderResponse : ReadBaseResponse
    {
        public CourseCapsuleForReorderResponseData? Data { get; set; }
    }
    public class CourseCapsuleForReorderResponseData
    {
        public GetCourseCapsuleForReorderResult? GetCourseCapsuleForReorderResult { get; set; }
    }
    public class GetCourseCapsuleForReorderResult
    {
        public int CourseCapsuleID { get; set; }
        public string? CourseCapsuleName { get; set; }
        public string? CourseCapsuleDesc { get; set; }
        public string? ImageURL { get; set; }
        public List<GetCourseCapsuleForReorderFolder>? CapsuleFolders { get; set; } = null;
    }
    public class GetCourseCapsuleForReorderFolder
    {
        public int CourseCapsuleFolderID { get; set; }
        public string? LessonName { get; set; }
        public string? LessonDesc { get; set; }
        public int DisplayOrder { get; set; }
        public List<GetCourseCapsuleForReorderPod>? CapsuleLessonPods { get; set; }
    }
    public class GetCourseCapsuleForReorderPod
    {
        public int CourseCapsuleLessonPodID { get; set; }
        public int LessonUnitID { get; set; }
        public string? LessonPodName { get; set; }
        public string? LessonPodDesc { get; set; }
        public string? Author { get; set; }
        public string? AccessType { get; set; }
        public int TotalActivities { get; set; }
        public int SelectedActivities { get; set; }
        public int AuthorID { get; set; }
        public int LessonPodVersion { get; set; }
        public int CapsuleLessonUnitID { get; set; }
        public string? CapsuleLessonName { get; set; }
        public int LessonPodDeleted { get; set; }
        public int LessonPodModified { get; set; }
        public int DisplayOrder { get; set; }
        public List<GetCourseCapsuleForReorderActivity>? CapsulePodActivities { get; set; }
    }
    public class GetCourseCapsuleForReorderActivity
    {
        public int CourseCapsuleActivityID { get; set; }
        public string? SlideName { get; set; }
        public string? SlideType { get; set; }
        public string? DocumentTitle { get; set; }
        public string? CapsuleSlideID { get; set; }
        public string? CapsuleSlideType { get; set; }
        public string? CapsuleSlideName { get; set; }
        public int IsSelected { get; set; }
        public string? ActivityType { get; set; }
        public int MinScore { get; set; }
        public int MinTimeMin { get; set; }
        public int MinTimeSec { get; set; }
        public int MaxTimeMin { get; set; }
        public int MaxTimeSec { get; set; }
        public int FollowTheFlow { get; set; }
        public int AutoHint { get; set; }
        public int ActivityDeleted { get; set; }
        public int CapsuleLessonUnitID { get; set; }
        public string? ContentMode { get; set; }
        public int DisplayOrder { get; set; }
    }
}
