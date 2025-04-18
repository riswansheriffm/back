using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace BackPack.Dependency.Library.Responses
{
    public class ReadBaseResponse
    {
        [SwaggerSchema("Response status ID")]
        [SwaggerSchemaExample("0")]
        public int MessageID { get; set; }

        [SwaggerSchema("Status code")]
        [SwaggerSchemaExample("200")]
        public int StatusCode { get; set; }

        [SwaggerSchema("Success")]
        [SwaggerSchemaExample("true")]
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerSchema("Message")]
        [SwaggerSchemaExample(CommonMessage.ReadMessage)]
        public string? StatusMessage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerIgnore]
        public string? ExceptionType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerIgnore]
        public string? ExceptionMessage { get; set; }
    }
}
