using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Grade
{
    public class DeleteGradeRequest
    {
        [SwaggerSchema("GradeID")]
        [SwaggerSchemaExample("10026")]
        public int GradeID { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }
    }
}
