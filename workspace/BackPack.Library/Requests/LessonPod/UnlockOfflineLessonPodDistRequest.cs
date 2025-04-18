using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class UnlockOfflineLessonPodDistRequest
    {
        [SwaggerSchema("Lesson unit dist ID")]
        [SwaggerSchemaExample("2132")]
        public int LessonUnitDistID { get; set; } = 0;

        [SwaggerSchema("Student ID")]
        [SwaggerSchemaExample("2132")]
        public int LoginID { get; set; } = 0;
    }
}
