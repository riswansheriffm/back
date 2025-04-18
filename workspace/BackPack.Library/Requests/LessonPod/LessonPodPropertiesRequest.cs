using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class LessonPodPropertiesRequest
    {
        [SwaggerSchema("Lesson unit ID")]
        [SwaggerSchemaExample("5167")]
        public int LessonUnitID { get; set; }

        [SwaggerSchema("Author ID")]
        [SwaggerSchemaExample("541")]
        public int AuthorID { get; set; }

        [SwaggerSchema("Lesson pod name")]
        [SwaggerSchemaExample("Service standardization Test 0051")]
        public string? LessonName { get; set; }

        [SwaggerSchema("Lesson pod description")]
        [SwaggerSchemaExample("Service standardization Test 0051")]
        public string? LessonDesc { get; set; }

        [SwaggerSchema("Access type Private/Public")]
        [SwaggerSchemaExample("Private")]
        public string? AccessType { get; set; }
    }
}
