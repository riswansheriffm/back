using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class UpdateLessonRequest
    {
        [SwaggerSchema("LessonID")]
        [SwaggerSchemaExample("1")]
        public int LessonID { get; set; }
        [SwaggerSchema("ContentID")]
        [SwaggerSchemaExample("1")]
        public int ContentID { get; set; }
        [SwaggerSchema("LessonName")]
        [SwaggerSchemaExample("test")]
        public string? LessonName { get; set; }
        [SwaggerSchema("LessonDesc")]
        [SwaggerSchemaExample("test")]
        public string? LessonDesc { get; set; }
        [SwaggerSchema("SubjectName")]
        [SwaggerSchemaExample("test")]
        public string? SubjectName { get; set; }
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("99")]
        public int SubjectID { get; set; }
        [SwaggerSchema("ActiveFlag")]
        [SwaggerSchemaExample("true")]
        public bool ActiveFlag  {get; set;} = true;
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("546")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("Tags")]
        [SwaggerSchemaExample("[\"2.1\", \"3.1\"]")]
        public List<string>? Tags { get; set; } 
        [SwaggerSchema("DeletedTags")]
        [SwaggerSchemaExample("[2,3]")]
        public List<int>? DeletedTags { get; set; }
        [SwaggerSchema("ImageURL")]
        [SwaggerSchemaExample("https://nodeserver.learnpods.com/f4109835eafa5d8c5f744516563e33cf.jpg")]
        public string? ImageURL { get; set; }
        [SwaggerSchema("ChapterID")]
        [SwaggerSchemaExample("7")]
        public int ChapterID { get; set; }
    }
}
