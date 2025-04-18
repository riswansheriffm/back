
namespace BackPack.MessageContract.Library
{
    public class GetAllCoursesByDomainEvent
    {
        public int DomainId { get; set; }
        public string DBConnection { get; set; } = string.Empty;
    }
}
