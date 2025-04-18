using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class DeleteLessonPodRequest
    {
        [SwaggerSchema("Author ID")]
        [SwaggerSchemaExample("541")]
        public int AuthorID { get; set; }

        [SwaggerSchema("Lesson pod ID")]
        [SwaggerSchemaExample("104")]
        public int LessonUnitID { get; set; }
    }
}
