using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Subject
{
    public class UpdateSubjectRequest
    {
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("357")]
        public int SubjectID { get; set; }
        [SwaggerSchema("GradeID")]
        [SwaggerSchemaExample("99")]
        public int GradeID { get; set; }
        [SwaggerSchema("SubjectName")]
        [SwaggerSchemaExample("Subject-3")]
        public string? SubjectName { get; set; }
        [SwaggerSchema("SubjectDesc")]
        [SwaggerSchemaExample("Subject-3")]
        public string? SubjectDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }
    }
}
