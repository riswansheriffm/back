using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class CreateLessonRequest
    {
        [SwaggerSchema("LessonName")]
        [SwaggerSchemaExample("test")]
        public string? LessonName { get; set; }
        [SwaggerSchema("LessonDesc")]
        [SwaggerSchemaExample("test")]
        public string? LessonDesc { get; set; }
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("99")]
        public int SubjectID { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("546")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("Tags")]
        [SwaggerSchemaExample("[\"2.1\", \"3.1\"]")]
        public List<string>? Tags { get; set; }
        [SwaggerSchema("ImageURL")]
        [SwaggerSchemaExample("https://nodeserver.learnpods.com/f4109835eafa5d8c5f744516563e33cf.jpg")]
        public string? ImageURL { get; set; }
        [SwaggerSchema("ChapterID")]
        [SwaggerSchemaExample("7")]
        public int ChapterID { get; set; }
    }
}
