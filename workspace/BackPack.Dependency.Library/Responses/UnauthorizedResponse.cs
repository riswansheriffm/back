
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace BackPack.Dependency.Library.Responses
{
    public class UnauthorizedResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerSchema("Response status ID")]
        [SwaggerSchemaExample("1")]
        public int? MessageID { get; set; }

        [SwaggerSchema("Success")]
        [SwaggerSchemaExample("false")]
        public bool Success { get; set; }

        [SwaggerSchema("Status Code")]
        [SwaggerSchemaExample("401")]
        public int StatusCode { get; set; }

        [SwaggerSchema("Message")]
        [SwaggerSchemaExample(CommonMessage.UnauthorizedMessage)]
        public string? StatusMessage { get; set; }
    }
}
