using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Tenant.Library.Requests
{
    public class DeleteTenantRequest
    {
        [SwaggerSchema("Tenant ID")]
        [SwaggerSchemaExample("8F996F6D-ABFE-493E-91E4-08FB180D06B0")]
        public Guid TenantID { get; set; }

        [SwaggerSchema("District ID")]
        [SwaggerSchemaExample("7489")]
        public int DomainID { get; set; }

        [SwaggerSchema("Activity By")]
        [SwaggerSchemaExample("88787")]
        public int ActivityBy { get; set; }
    }
}
