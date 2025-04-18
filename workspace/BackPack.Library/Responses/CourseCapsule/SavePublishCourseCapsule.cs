
namespace BackPack.Library.Responses.CourseCapsule
{
    public class SavePublishCourseCapsule
    {
        public int DomainID { get; set; }
        public int LoginID { get; set; }
        public int SubjectID { get; set; }
        public int CourseCapsuleID { get; set; }
        public string? CourseCapsuleName { get; set; }
        public string? CourseCapsuleDesc { get; set; }
        public string? ImageURL { get; set; }
        public int PublishType { get; set; }
        public string? AppType { get; set; }
    }
}
