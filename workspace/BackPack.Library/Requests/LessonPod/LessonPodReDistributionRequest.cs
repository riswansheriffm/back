using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class LessonPodReDistributionRequest : LessonPodDistributionRequest
    {
        [SwaggerSchema("Lesson pod distribution ID")]
        [SwaggerSchemaExample("2278")]
        public int LessonUnitDistID { get; set; }

        [SwaggerSchema("Course ID")]
        [SwaggerSchemaExample("1")]
        public int CourseID { get; set; }
    }
}
