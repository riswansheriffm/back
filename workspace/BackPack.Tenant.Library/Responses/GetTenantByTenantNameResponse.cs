using BackPack.Dependency.Library.Responses;

namespace BackPack.Tenant.Library.Responses
{
    public class GetTenantByTenantNameResponse : ReadBaseResponse
    {
        public GetTenantByTenantNameData Data { get; set; } = new();
    }
    public class GetTenantByTenantNameData
    {
        public Guid TenantID { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string DBConnection { get; set; } = string.Empty;
    }
}
