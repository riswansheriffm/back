
namespace BackPack.MessageContract.Library
{
    public class GetCourseCapsuleByCapsuleEvent
    {
        public int DomainId { get; set; }
        public int CourseCapsuleId { get; set; }
        public string DBConnection { get; set; } = string.Empty;
    }
}
