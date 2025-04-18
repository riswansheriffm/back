using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.School
{
    public class DeleteSchoolRequest
    {
        [SwaggerSchema("School ID")]
        [SwaggerSchemaExample("7489")]
        public int? ID { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("Activity Desc")]
        public string? ActivityDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("10")]
        public int? ActivityBy { get; set; }
    }
}
