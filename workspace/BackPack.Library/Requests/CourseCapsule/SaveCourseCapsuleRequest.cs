using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class SaveCourseCapsuleRequest
    {
        [SwaggerSchema("DomainID")]
        [SwaggerSchemaExample("104")]
        public int DomainID { get; set; }
        [SwaggerSchema("LoginID")]
        [SwaggerSchemaExample("546")]
        public int LoginID { get; set; }
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("99")]
        public int SubjectID { get; set; }
        [SwaggerSchema("CourseCapsuleName")]
        [SwaggerSchemaExample("test 1")]
        public string? CourseCapsuleName { get; set; }
        [SwaggerSchema("CourseCapsuleDesc")]
        [SwaggerSchemaExample("test 1")]
        public string? CourseCapsuleDesc { get; set; }
        [SwaggerSchema("ImageURL")]
        [SwaggerSchemaExample("https://nodeserver.learnpods.com/f4109835eafa5d8c5f744516563e33cf.jpg")]
        public string? ImageURL { get; set; }
        [SwaggerSchema("CourseCapsuleID")]
        [SwaggerSchemaExample("1048")]
        public int CourseCapsuleID { get; set; }
        [SwaggerSchema("AppType")]
        [SwaggerSchemaExample("Knomadix")]
        public string? AppType { get; set; }
    }
}
