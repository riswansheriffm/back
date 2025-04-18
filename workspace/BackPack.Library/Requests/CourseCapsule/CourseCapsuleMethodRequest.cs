
using BackPack.Library.Responses.CourseCapsule;
using BackPack.Library.Responses.LessonPod.Distribution;
using Npgsql;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class CourseCapsuleMethodRequest
    {
        public int LoginId { get; set; }
        public SavePublishCourseCapsule CourseCapsule { get; set; } = new();
        public SavePublishCourseCapsuleLessonPod CourseCapsuleLessonPod { get; set; } = new();
        public CourseCapsuleSlide CourseCapsuleActivity { get; set; } = new();
        public int PublishCourseCapsuleId { get; set; }
        public LessonPodSlideResponse LessonPodSlideResponse { get; set; } = new();
        public int ParentContentId {  get; set; } 
        public required NpgsqlTransaction SqlTransaction { get; set; }
        public required NpgsqlConnection DbConnection { get; set; }
        public string LessonJson { get; set; } = string.Empty;
    }
}
