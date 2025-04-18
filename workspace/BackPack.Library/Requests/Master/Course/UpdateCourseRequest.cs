using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Course
{
    public class UpdateCourseRequest
    {
        [SwaggerSchema("CourseID")]
        [SwaggerSchemaExample("357")]
        public int CourseID { get; set; }
        [SwaggerSchema("CourseName")]
        [SwaggerSchemaExample("Course-3")]
        public string? CourseName { get; set; }
        [SwaggerSchema("CourseDesc")]
        [SwaggerSchemaExample("Course-3")]
        public string? CourseDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }
        [SwaggerSchema("ImageURL")]
        [SwaggerSchemaExample("1")]
        public string? ImageURL { get; set; }
        [SwaggerSchema("CourseType")]
        [SwaggerSchemaExample("0")]
        public int CourseType { get; set; }

        [SwaggerSchema("Teacher list")]
        [SwaggerSchemaExample("[1,2,3]")]
        public List<int> TeachersList { get; set; } = [];

        [SwaggerSchema("Student list")]
        [SwaggerSchemaExample("[1,2,3]")]
        public List<int> StudentsList { get; set; } = [];
    }
}
