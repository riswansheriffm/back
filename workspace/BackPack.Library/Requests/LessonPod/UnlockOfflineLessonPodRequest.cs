using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class UnlockOfflineLessonPodRequest
    {
        [SwaggerSchema("Student ID")]
        [SwaggerSchemaExample("2132")]
        public int LoginID { get; set; } = 0;

        [SwaggerSchema("Course ID")]
        [SwaggerSchemaExample("2132")]
        public int CourseID { get; set; } = 0;
    }
}
