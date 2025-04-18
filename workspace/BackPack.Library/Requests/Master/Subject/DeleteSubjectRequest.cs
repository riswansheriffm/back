using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Subject
{
    public class DeleteSubjectRequest
    {
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("10026")]
        public int SubjectID { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }
    }
}
