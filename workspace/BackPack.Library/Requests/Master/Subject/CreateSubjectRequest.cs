using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Subject
{
    public class CreateSubjectRequest
    {
        [SwaggerSchema("DistrictID")]
        [SwaggerSchemaExample("1381")]
        public int DistrictID { get; set; }
        [SwaggerSchema("SubjectName")]
        [SwaggerSchemaExample("Subject 1")]
        public string? SubjectName { get; set; }
        [SwaggerSchema("SubjectDesc")]
        [SwaggerSchemaExample("Subject 1 desc")]
        public string? SubjectDesc { get; set; }
        [SwaggerSchema("GradeID")]
        [SwaggerSchemaExample("1")]
        public int GradeID { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }

    }
}
