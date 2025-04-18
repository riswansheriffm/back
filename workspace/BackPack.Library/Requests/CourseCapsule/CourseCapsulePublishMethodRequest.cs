
using Npgsql;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class CourseCapsulePublishMethodRequest
    {
        public int LoginId { get; set; }

        public int CourseCapsuleActivityID { get; set; }

        public int PublishedVersion { get; set; }

        public int PublishCourseCapsuleLessonpodId { get; set; }

        public int ContentId { get; set; }

        public int CourseCapsuleActivityId { get; set; }

        public int PodActivityDisplayOrder { get; set; }

        public required NpgsqlTransaction SqlTransaction { get; set; }

        public required NpgsqlConnection DbConnection { get; set; }

        public string SlideInputJson { get; set; } = string.Empty;
    }
}
