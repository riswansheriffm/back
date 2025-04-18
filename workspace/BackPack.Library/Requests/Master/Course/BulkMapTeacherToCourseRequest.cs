using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Course
{
    public class BulkMapTeacherToCourseRequest
    {
        [SwaggerSchema("DomainID")]
        [SwaggerSchemaExample("6440")]
        public int DomainID { get; set; }
        public List<TeacherToCourse>? ListTeacherToCourse { get; set; } 
    }
    public class TeacherToCourse
    {
        [SwaggerSchema("LoginName")]
        [SwaggerSchemaExample("ssd")]
        public string? LoginName { get; set; }
        [SwaggerSchema("CourseName")]
        [SwaggerSchemaExample("ssi")]
        public string? CourseName { get; set; }
    }
}
