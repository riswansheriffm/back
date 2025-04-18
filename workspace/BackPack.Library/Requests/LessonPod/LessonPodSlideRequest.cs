using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class LessonPodSlideRequest
    {
        [SwaggerSchema("Content ID")]
        [SwaggerSchemaExample("0")]
        public int ContentID { get; set; } = 0;

        [SwaggerSchema("NO. of items")]
        [SwaggerSchemaExample("0")]
        public int NoOfItems { get; set; } = 0;

        [SwaggerSchema("Attempts")]
        [SwaggerSchemaExample("0")]
        public int Attempts { get; set; } = 0;

        [SwaggerSchema("Minimum time")]
        [SwaggerSchemaExample("0")]
        public int MinTime { get; set; } = 0;

        [SwaggerSchema("Maximum time")]
        [SwaggerSchemaExample("0")]
        public int MaxTime { get; set; } = 0;

        [SwaggerSchema("Mastery percentage")]
        [SwaggerSchemaExample("0")]
        public int MasteryPercentage { get; set; } = 0;

        [SwaggerSchema("Follow the flow sequence")]
        [SwaggerSchemaExample("1")]
        public int FollowTheFlow { get; set; } = 0;

        [SwaggerSchema("Randomize")]
        [SwaggerSchemaExample("0")]
        public int Randomize { get; set; } = 0;

        [SwaggerSchema("Auto hint")]
        [SwaggerSchemaExample("1")]
        public int AutoHint { get; set; } = 0;

        [SwaggerSchema("Optional")]
        [SwaggerSchemaExample("false")]
        public bool Optional { get; set; } = false;

        [SwaggerSchema("Slide ID")]
        [SwaggerSchemaExample("pa-1688116384691")]
        public string SlideID { get; set; } = string.Empty;

        [SwaggerSchema("Activity type")]
        [SwaggerSchemaExample("practice")]
        public string ActivityType { get; set; } = string.Empty;

        [SwaggerSchema("Content mode")]
        [SwaggerSchemaExample("ALL")]
        public string ContentMode { get; set; } = string.Empty;

        [SwaggerSchema("Target date of completion")]
        [SwaggerSchemaExample("2023-07-29")]
        public string TargetDateOfCompletion { get; set; } = string.Empty;

        [SwaggerSchema("Target time of completion")]
        [SwaggerSchemaExample("00:00")]
        public string TargetTimeOfCompletion { get; set; } = string.Empty;

        [SwaggerSchema("Parent slide ID")]
        [SwaggerSchemaExample("")]
        public string ParentSlideID { get; set; } = string.Empty;
    }
}
