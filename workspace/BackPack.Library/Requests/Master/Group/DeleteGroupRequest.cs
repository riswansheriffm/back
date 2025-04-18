using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Group
{
    public class DeleteGroupRequest
    {
        [SwaggerSchema("Group ID")]
        [SwaggerSchemaExample("2712")]
        public int GroupID { get; set; }

        [SwaggerSchema("Creater ID")]
        [SwaggerSchemaExample("541")]
        public int ActivityBy { get; set; }
    }
}
