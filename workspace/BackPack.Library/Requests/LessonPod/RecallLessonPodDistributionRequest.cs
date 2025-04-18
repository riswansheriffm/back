using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class RecallLessonPodDistributionRequest
    {
        [SwaggerSchema("Author ID")]
        [SwaggerSchemaExample("541")]
        public int LoginID { get; set; }

        [SwaggerSchema("Lesson pod distribution ID")]
        [SwaggerSchemaExample("104")]
        public int LessonUnitDistID { get; set; }
    }
}
