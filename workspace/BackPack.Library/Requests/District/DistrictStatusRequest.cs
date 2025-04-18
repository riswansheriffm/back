using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.District
{
    public class DistrictStatusRequest
    {
        [SwaggerSchema("District ID")]
        [SwaggerSchemaExample("7489")]
        public int ID { get; set; }
        [SwaggerSchema("Activity By")]
        [SwaggerSchemaExample("88787")]
        public int ActivityBy { get; set; }
    }
}
