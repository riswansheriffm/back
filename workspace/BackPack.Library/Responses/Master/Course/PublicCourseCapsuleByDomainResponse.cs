using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class PublicCourseCapsuleByDomainResponse : ReadBaseResponse
    {
        public PublicCourseCapsuleByDomainResponseData Data { get; set; } = new();
    }
    public class PublicCourseCapsuleByDomainResponseData
    {
        public List<PublicCourseCapsuleByDomain> GetAllPublicCourseCapsuleByDomainAndSubjectResult { get; set; } = [];
    }
    public class PublicCourseCapsuleByDomain
    {
        public int PublishCourseCapsuleID { get; set; }
        public int CourseCapsuleID { get; set; }
        public string? CourseCapsuleName { get; set; }
        public string? CourseCapsuleDesc { get; set; }  
    }
}
