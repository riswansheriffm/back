
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class PublishCourseCapsuleRequest
    {
        [SwaggerSchema("LoginID")]
        [SwaggerSchemaExample("546")]
        public int LoginID { get; set; }
        [SwaggerSchema("CourseCapsuleID")]
        [SwaggerSchemaExample("1003")]
        public int CourseCapsuleID { get; set; }
        [SwaggerSchema("PublishType")]
        [SwaggerSchemaExample("1")]
        public int PublishType { get; set; }
    }
}
