using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class UpdateCourseCapsulePodRequest
    {
        [SwaggerSchema("CourseCapsuleFolderID")]
        [SwaggerSchemaExample("2")]
        public int CourseCapsuleFolderID { get; set; }
        public List<UpdateCourseCapsulePodReorderList>? Pods { get; set; }
    }
    public class UpdateCourseCapsulePodReorderList
    {
        [SwaggerSchema("CourseCapsuleLessonPodID")]
        [SwaggerSchemaExample("3")]
        public int CourseCapsuleLessonPodID { get; set; }
        [SwaggerSchema("DisplayOrder")]
        [SwaggerSchemaExample("6")]
        public int DisplayOrder { get; set; }
    }
}
