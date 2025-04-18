
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.ClassLink
{
    public class CreateClassLinkRequest
    {
        [SwaggerSchema("Domain ID")]
        [SwaggerSchemaExample("10")]
        public int DomainID { get; set; }
    }

}
