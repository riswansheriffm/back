using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Group
{
    public class UpdateGroupRequest : CreateGroupRequest
    {
        [SwaggerSchema("Group ID")]
        [SwaggerSchemaExample("1130")]
        public int GroupID { get; set; }
    }
}
