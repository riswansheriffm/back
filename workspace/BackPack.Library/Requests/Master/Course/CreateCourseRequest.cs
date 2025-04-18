using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Course
{
    public class CreateCourseRequest
    {
        [SwaggerSchema("DistrictID")]
        [SwaggerSchemaExample("1381")]
        public int DistrictID { get; set; }
        [SwaggerSchema("CourseName")]
        [SwaggerSchemaExample("Course 1")]
        public string? CourseName { get; set; }
        [SwaggerSchema("CourseDesc")]
        [SwaggerSchemaExample("Course 1 desc")]
        public string? CourseDesc { get; set; }
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("99")]
        public int SubjectID { get; set; }
        [SwaggerSchema("SchoolID")]
        [SwaggerSchemaExample("95")]
        public int SchoolID { get; set; }
        [SwaggerSchema("CourseType")]
        [SwaggerSchemaExample("0")]
        public int CourseType { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }
        [SwaggerSchema("ImageURL")]
        [SwaggerSchemaExample("1")]
        public string? ImageURL { get; set; }
    }
}
