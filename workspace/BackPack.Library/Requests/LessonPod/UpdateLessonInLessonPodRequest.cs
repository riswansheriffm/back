using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class UpdateLessonInLessonPodRequest
    {
        [SwaggerSchema("Author ID")]
        [SwaggerSchemaExample("541")]
        public int LoginID { get; set; }

        [SwaggerSchema("Lesson ID")]
        [SwaggerSchemaExample("1072")]
        public int LessonID { get; set; }

        [SwaggerSchema("Lesson unit ID")]
        [SwaggerSchemaExample("5188")]
        public int LessonUnitID { get; set; }
    }
}
