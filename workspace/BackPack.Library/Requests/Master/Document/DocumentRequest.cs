using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Requests.School;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Master.Document
{
    public class DocumentRequest
    {
        public List<DocumentList> documents { get; set; } = new List<DocumentList>();       
    }
    public class DocumentList 
    {
        [SwaggerSchema("title")]
        [SwaggerSchemaExample("AWS")]
        public string? title { get; set; }

        [SwaggerSchema("embedding")]
        [SwaggerSchemaExample("[0.1, 0.2, 0.3, 0.4]")]
        public float[]? embedding { get; set; }

        [SwaggerSchema("content")]
        [SwaggerSchemaExample("Sample document content")]
        public string? content { get; set; }

        [SwaggerSchema("source")]
        [SwaggerSchemaExample("aws.pdf")]
        public string? source { get; set; }

        [SwaggerSchema("page")]
        [SwaggerSchemaExample("1")]
        public int page { get; set; }
    }
}  
