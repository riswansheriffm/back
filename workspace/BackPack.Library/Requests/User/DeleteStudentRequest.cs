
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class DeleteStudentRequest
    {
        [SwaggerSchema("Student ID")]
        [SwaggerSchemaExample("7489")]
        public int? ID { get; set; }

        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("10")]
        public int? ActivityBy { get; set; }
    }
}
