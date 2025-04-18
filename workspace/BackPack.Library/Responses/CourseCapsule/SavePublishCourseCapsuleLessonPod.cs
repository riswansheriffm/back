
namespace BackPack.Library.Responses.CourseCapsule
{
    public class SavePublishCourseCapsuleLessonPod
    {
        public int PublishCourseCapsuleID { get; set; }
        public int LessonUnitID { get; set; }
        public int LessonID { get; set; }
        public string? LessonName { get; set; }
        public string? LessonDesc { get; set; }
        public string? ImageURL { get; set; }
        public int PublishedVersion { get; set; }
        public int CourseCapsuleLessonPodID { get; set; }
        public int LoginID { get; set; }
        public int CourseCapsuleFolderID { get; set; }
        public int DisplayOrder { get; set; }
        public string? LessonJson { get; set; }
    }
}
