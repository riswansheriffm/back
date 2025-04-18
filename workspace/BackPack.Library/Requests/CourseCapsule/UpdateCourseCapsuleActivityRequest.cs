using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class UpdateCourseCapsuleActivityRequest
    {
        [SwaggerSchema("CourseCapsuleLessonPodID")]
        [SwaggerSchemaExample("2")]
        public int CourseCapsuleLessonPodID { get; set; }
        public List<UpdateCourseCapsuleActivityReorderList>? Activities { get; set; }
    }
    public class UpdateCourseCapsuleActivityReorderList
    {
        [SwaggerSchema("CourseCapsuleActivityID")]
        [SwaggerSchemaExample("6")]
        public int CourseCapsuleActivityID { get; set; }
        [SwaggerSchema("DisplayOrder")]
        [SwaggerSchemaExample("6")]
        public int DisplayOrder { get; set; }
    }
}
