using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class CreateCourseCapsuleLicenseRequest
    {
        [SwaggerSchema("DomainID")]
        [SwaggerSchemaExample("6")]
        public int DomainID { get; set; }
        [SwaggerSchema("LoginID")]
        [SwaggerSchemaExample("26")]
        public int LoginID { get; set; }
        [SwaggerSchema("CourseCapsuleID")]
        [SwaggerSchemaExample("1026")]
        public int CourseCapsuleID { get; set; }
        [SwaggerSchema("CourseID")]
        [SwaggerSchemaExample("2")]
        public int CourseID { get; set; }
        [SwaggerSchema("CourseIDs")]
        [SwaggerSchemaExample("[2]")]
        public List<int>? CourseIDs { get; set; }
        [SwaggerSchema("StudentIDs")]
        [SwaggerSchemaExample("[]")]
        public List<int>? StudentIDs { get; set; }
        [SwaggerSchema("Duration")]
        [SwaggerSchemaExample("1")]
        public int Duration { get; set; }
        [SwaggerSchema("WhomToLicense")]
        [SwaggerSchemaExample("all")]
        public string? WhomToLicense { get; set; }
        [SwaggerSchema("StartDate")]
        [SwaggerSchemaExample("2022-08-17")]
        public DateTime StartDate { get; set; }
        [SwaggerSchema("LicenseAction")]
        [SwaggerSchemaExample("Subscribe")]
        public string? LicenseAction { get; set; }
        [SwaggerSchema("LicenseType")]
        [SwaggerSchemaExample("Public")]
        public string? LicenseType { get; set; }
    }
}
