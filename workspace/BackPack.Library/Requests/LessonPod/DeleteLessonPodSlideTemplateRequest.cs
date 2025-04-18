using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class DeleteLessonPodSlideTemplateRequest
    {
        [SwaggerSchema("Slide template ID")]
        [SwaggerSchemaExample("0")]
        public int SlideTemplateID { get; set; }

        [SwaggerSchema("Domain ID")]
        [SwaggerSchemaExample("104")]
        public int DomainID { get; set; }

        [SwaggerSchema("Login ID")]
        [SwaggerSchemaExample("541")]
        public int LoginID { get; set; }
    }
}
