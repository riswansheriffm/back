using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class CopyLessonPodRequest
    {
        [SwaggerSchema("Lesson unit ID")]
        [SwaggerSchemaExample("5168")]
        public int LessonUnitID { get; set; }

        [SwaggerSchema("Author ID")]
        [SwaggerSchemaExample("541")]
        public int AuthorID { get; set; }

        [SwaggerSchema("Lesson pod name")]
        [SwaggerSchemaExample("Service standardization Test 011 copy")]
        public string? LessonName { get; set; }

        [SwaggerSchema("Lesson pod description")]
        [SwaggerSchemaExample("Service standardization Test 011 copy")]
        public string? LessonDesc { get; set; }
    }
}
