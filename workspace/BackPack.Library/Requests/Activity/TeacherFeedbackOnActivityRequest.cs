using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Activity
{
    public class TeacherFeedbackOnActivityRequest
    {
        [SwaggerSchema("Student ID")]
        [SwaggerSchemaExample("21380")]
        public int? StudentID { get; set; }

        [SwaggerSchema("Content ID")]
        [SwaggerSchemaExample("22401")]
        public int? ContentID { get; set; }

        [SwaggerSchema("Teacher ID")]
        [SwaggerSchemaExample("541")]
        public int? AuthorID { get; set; }

        [SwaggerSchema("Rework")]
        [SwaggerSchemaExample("0")]
        public int? Rework { get; set; }

        [SwaggerSchema("Teacher feedback")]
        [SwaggerSchemaExample("Rework")]
        public string? Feedback { get; set; }

        [SwaggerSchema("Grade")]
        [SwaggerSchemaExample("1")]
        public int Grade { get; set; }

        [SwaggerSchema("Remarks")]
        [SwaggerSchemaExample("Remarks")]
        public string? Remarks { get; set; }

        [SwaggerSchema("Teacher Recording string")]
        [SwaggerSchemaExample("")]
        public string? TeacherRecording { get; set; }

        [SwaggerSchema("Teacher Ink string")]
        [SwaggerSchemaExample("")]
        public string? TeacherInk { get; set; }
    }
}
