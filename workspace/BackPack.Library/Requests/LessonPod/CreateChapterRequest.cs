using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class CreateChapterRequest
    {
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("1381")]
        public int SubjectID { get; set; }
        [SwaggerSchema("ChapterName")]
        [SwaggerSchemaExample("Chapter-3")]
        public string? ChapterName { get; set; }
        [SwaggerSchema("ChapterDesc")]
        [SwaggerSchemaExample("Chapter-3")]
        public string? ChapterDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("1")]
        public string? ActivityDesc { get; set; }
        [SwaggerSchema("ImageURL")]
        [SwaggerSchemaExample("https://nodeserver.learnpods.com/f4109835eafa5d8c5f744516563e33cf.jpg")]
        public string? ImageURL { get; set; }
    }
}
