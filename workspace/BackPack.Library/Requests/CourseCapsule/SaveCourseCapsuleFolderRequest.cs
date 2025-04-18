using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class SaveCourseCapsuleFolderRequest
    {
        [SwaggerSchema("CourseCapsuleID")]
        [SwaggerSchemaExample("1002")]
        public int CourseCapsuleID { get; set; }
        [SwaggerSchema("LoginID")]
        [SwaggerSchemaExample("546")]
        public int LoginID { get; set; }
        public List<SaveCourseCapsuleFolderListObject>? Folder { get; set; }
    }
    public class SaveCourseCapsuleFolderListObject
    {
        [SwaggerSchema("CourseCapsuleFolderID")]
        [SwaggerSchemaExample("78")]
        public int CourseCapsuleFolderID { get; set; }
        [SwaggerSchema("FolderName")]
        [SwaggerSchemaExample("test")]
        public string? FolderName { get; set; }
        [SwaggerSchema("FolderDesc")]
        [SwaggerSchemaExample("test")]
        public string? FolderDesc { get; set; }
    }
}
