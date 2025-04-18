using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Grade
{
    public class CreateGradeRequest
    {
        [SwaggerSchema("DistrictID")]
        [SwaggerSchemaExample("1381")]
        public int DistrictID { get; set; }
        [SwaggerSchema("GradeName")]
        [SwaggerSchemaExample("Grade 1")]
        public string? GradeName { get; set; }
        [SwaggerSchema("GradeDesc")]
        [SwaggerSchemaExample("grade 1 desc")]
        public string? GradeDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }

    }
}
