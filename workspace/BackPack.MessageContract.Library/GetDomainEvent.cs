namespace BackPack.MessageContract.Library
{
    public class GetDomainEvent
    {
        public int DomainID { get; set; }
        public string? DBConnection { get; set; }
    }
}
