
using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{  
    public class PublicCourseCapsuleByDomainResponseEvent : ReadBaseResponse
    {
        public PublicCourseCapsuleByDomainResponseEventData Data { get; set; } = new();
    }
    public class PublicCourseCapsuleByDomainResponseEventData
    {
        public List<PublicCourseCapsuleByDomainEvent> GetAllPublicCourseCapsuleByDomainAndSubjectResult { get; set; } = [];
    }
    public class PublicCourseCapsuleByDomainEvent
    {
        public int PublishCourseCapsuleID { get; set; }
        public int CourseCapsuleID { get; set; }
        public string? CourseCapsuleName { get; set; }
        public string? CourseCapsuleDesc { get; set; }
    }
}
