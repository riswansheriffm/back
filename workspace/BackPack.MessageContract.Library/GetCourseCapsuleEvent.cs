
namespace BackPack.MessageContract.Library
{
    public class GetCourseCapsuleEvent
    {
        public int DomainId { get; set; }
        public int LoginId { get; set; }
        public int SubjectId { get; set; }
        public string DBConnection { get; set; } = string.Empty;
    }
}
