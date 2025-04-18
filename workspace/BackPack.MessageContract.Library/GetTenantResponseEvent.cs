namespace BackPack.MessageContract.Library
{
    public class GetTenantResponseEvent
    {
        public Guid TenantID { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string DBConnection { get; set; } = string.Empty;
    }
}
