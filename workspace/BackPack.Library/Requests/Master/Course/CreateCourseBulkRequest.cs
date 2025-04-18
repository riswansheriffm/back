using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Course
{
    public class CreateCourseBulkRequest
    {
        [SwaggerSchema("SchoolID")]
        [SwaggerSchemaExample("6440")]
        public int SchoolID { get; set; }
        [SwaggerSchema("CourseList")]
        [SwaggerSchemaExample("[{\"CourseName\":\"sld\",\"CourseDescrption\":\"sd\",\"SubjectName\":\"sdf\"}]")]

        public List<BulkCourseObject> CourseList { get; set; } = new List<BulkCourseObject>();
    }
    public class BulkCourseObject
    {
        [SwaggerSchema("Course Name")]
        [SwaggerSchemaExample("DName")]
        public string? CourseName { get; set; }
        [SwaggerSchema("Course Description")]
        [SwaggerSchemaExample("DDesc")]
        public string? CourseDescrption { get; set; }
        [SwaggerSchema("SubjectName")]
        [SwaggerSchemaExample("SN")]
        public string? SubjectName { get; set; }


    }
}
