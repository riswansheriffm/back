using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class DeleteCourseCapsuleRequest
    {
        [SwaggerSchema("LoginID")]
        [SwaggerSchemaExample("0")]
        public int LoginID { get; set; }
        [SwaggerSchema("CourseCapsuleID")]
        [SwaggerSchemaExample("1053")]
        public int CourseCapsuleID { get; set; }
    }
}
