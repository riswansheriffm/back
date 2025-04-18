
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace BackPack.Dependency.Library.Responses
{
    public class BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerSchema("Response status ID")]
        [SwaggerSchemaExample("0")]
        public int? MessageID { get; set; }

        [SwaggerSchema("Status code")]
        [SwaggerSchemaExample("201")]
        public int StatusCode { get; set; }

        [SwaggerSchema("Success")]
        [SwaggerSchemaExample("true")]
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerSchema("Status message")]
        [SwaggerSchemaExample(CommonMessage.CreatedMessage)]
        public string? StatusMessage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerIgnore]
        public string? ExceptionType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerIgnore]
        public string? ExceptionMessage { get; set; }

        [JsonIgnore]
        [SwaggerIgnore]
        public int ResultCount { get; set; }

        [JsonIgnore]
        [SwaggerIgnore]
        public string? DomainName { get; set; }
    }
}
