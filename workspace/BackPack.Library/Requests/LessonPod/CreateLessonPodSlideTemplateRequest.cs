using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class CreateLessonPodSlideTemplateRequest
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

        [SwaggerSchema("Bold")]
        [SwaggerSchemaExample("true")]
        public bool Bold { get; set; }

        [SwaggerSchema("Italic")]
        [SwaggerSchemaExample("false")]
        public bool Italic { get; set; }

        [SwaggerSchema("Template name")]
        [SwaggerSchemaExample("Smart paper")]
        public string TemplateName { get; set; } = string.Empty;

        [SwaggerSchema("Access type")]
        [SwaggerSchemaExample("Private")]
        public string AccessType { get; set; } = string.Empty;

        [SwaggerSchema("Question color")]
        [SwaggerSchemaExample("Smart paper")]
        public string QuestionColor { get; set; } = string.Empty;

        [SwaggerSchema("Answer Color")]
        [SwaggerSchemaExample("Smart paper")]
        public string AnswerColor { get; set; } = string.Empty;

        [SwaggerSchema("Font Style")]
        [SwaggerSchemaExample("Smart paper")]
        public string FontStyle { get; set; } = string.Empty;

        [SwaggerSchema("Question Font Size")]
        [SwaggerSchemaExample("Smart paper")]
        public string QuestionFontSize { get; set; } = string.Empty;

        [SwaggerSchema("Answer Font Size")]
        [SwaggerSchemaExample("Smart paper")]
        public string AnswerFontSize { get; set; } = string.Empty;

        [SwaggerSchema("Opacity")]
        [SwaggerSchemaExample("Smart paper")]
        public string Opacity { get; set; } = string.Empty;

        [SwaggerSchema("Selection Color")]
        [SwaggerSchemaExample("Smart paper")]
        public string SelectionColor { get; set; } = string.Empty;

        [SwaggerSchema("Answer Fill Color")]
        [SwaggerSchemaExample("Smart paper")]
        public string AnswerFillColor { get; set; } = string.Empty;

        [SwaggerSchema("Question Fill Color")]
        [SwaggerSchemaExample("Smart paper")]
        public string QuestionFillColor { get; set; } = string.Empty;

        [SwaggerSchema("Background")]
        [SwaggerSchemaExample("Smart paper")]
        public string Background { get; set; } = string.Empty;
    }
}
