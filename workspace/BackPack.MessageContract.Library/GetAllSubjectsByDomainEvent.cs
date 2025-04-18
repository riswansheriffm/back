
namespace BackPack.MessageContract.Library
{
    public class GetAllSubjectsByDomainEvent
    {
        public int DomainId { get; set; }
        public int LoginId { get; set; }
        public string DBConnection { get; set; } = string.Empty;
    }
}
