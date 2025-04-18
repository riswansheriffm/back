using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class UpdateCourseCapsuleFolderRequest
    {
        [SwaggerSchema("CourseCapsuleID")]
        [SwaggerSchemaExample("1")]
        public int CourseCapsuleID { get; set; }
        public List<UpdateCourseCapsuleFolderReorderList>? Folders { get; set; }
    }
    public class UpdateCourseCapsuleFolderReorderList
    {
        [SwaggerSchema("CourseCapsuleFolderID")]
        [SwaggerSchemaExample("1")]
        public int CourseCapsuleFolderID { get; set; }
        [SwaggerSchema("DisplayOrder")]
        [SwaggerSchemaExample("6")]
        public int DisplayOrder { get; set; }
    }
}
