using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class DeleteLessonRequest
    {
        [SwaggerSchema("LessonID")]
        [SwaggerSchemaExample("3900")]
        public int LessonID { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("06")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("Deleted Lesson")]
        public string? ActivityDesc { get; set; }
    }
}
