using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Group
{
    public class CreateGroupRequest
    {
        [SwaggerSchema("Course ID")]
        [SwaggerSchemaExample("2712")]
        public int CourseID { get; set; }

        [SwaggerSchema("Creater ID")]
        [SwaggerSchemaExample("541")]
        public int ActivityBy { get; set; }

        [SwaggerSchema("Group name")]
        [SwaggerSchemaExample("ClassAGroup")]
        public string? GroupName { get; set; }

        [SwaggerSchema("Group description")]
        [SwaggerSchemaExample("ClassA group")]
        public string? GroupDesc { get; set; }

        [SwaggerSchema("List of students")]
        [SwaggerSchemaExample("[21380,21381]")]
        public List<int>? StudentsList { get; set; }
    }
}
