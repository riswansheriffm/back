using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Grade
{
    public class UpdateGradeRequest
    {
        [SwaggerSchema("GradeID")]
        [SwaggerSchemaExample("10026")]
        public int GradeID { get; set; }
        [SwaggerSchema("GradeName")]
        [SwaggerSchemaExample("Grade-3")]
        public string? GradeName { get; set; }
        [SwaggerSchema("GradeDesc")]
        [SwaggerSchemaExample("Grade-3")]
        public string? GradeDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }
    }
}
