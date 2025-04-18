namespace BackPack.MessageContract.Library
{
    public class DeleteDomainEvent
    {
        public Guid TenantID { get; set; }
        public int DomainID { get; set; }
        public string? DBConnection { get; set; }
        public int ActivityBy { get; set; }
    }
}
