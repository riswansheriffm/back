using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.LessonPod
{
    public class LessonPodDistributionRequest
    {
        [SwaggerSchema("Doamain ID")]
        [SwaggerSchemaExample("104")]
        public int DomainID { get; set; } = 0;

        [SwaggerSchema("Lesson unit ID")]
        [SwaggerSchemaExample("5164")]
        public int LessonUnitID { get; set; } = 0;

        [SwaggerSchema("Course IDs")]
        [SwaggerSchemaExample("[2712, 2]")]
        public List<int> CourseIDs { get; set; } = [];

        [SwaggerSchema("Author ID")]
        [SwaggerSchemaExample("541")]
        public int AuthorID { get; set; } = 0;

        [SwaggerSchema("Visible to parent")]
        [SwaggerSchemaExample("false")]
        public int FlagVisibleToParent { get; set; } = 0;

        [SwaggerSchema("Group IDs")]
        [SwaggerSchemaExample("[18, 19, 21]")]
        public List<int> GroupIDs { get; set; } = new List<int>();

        [SwaggerSchema("Student IDs")]
        [SwaggerSchemaExample("[]")]
        public List<int> StudentIDs { get; set; } = new List<int>();

        [SwaggerSchema("Lesson name")]
        [SwaggerSchemaExample("Sample lesson unit")]
        public string LessonName { get; set; } = string.Empty;

        [SwaggerSchema("Lesson description")]
        [SwaggerSchemaExample("Sample lesson unit description")]
        public string LessonDesc { get; set; } = string.Empty;

        [SwaggerSchema("Target date of completion")]
        [SwaggerSchemaExample("2023-07-29")]
        public string TargetDateOfCompletion { get; set; } = string.Empty;

        [SwaggerSchema("Target time of completion")]
        [SwaggerSchemaExample("12:00")]
        public string TargetTimeOfCompletion { get; set; } = string.Empty;

        [SwaggerSchema("Whom to distribute ALL/SelectGroups/SelectStudents")]
        [SwaggerSchemaExample("SelectGroups")]
        public string WhomToDistribute { get; set; } = string.Empty;

        [SwaggerSchema("Lesson type")]
        [SwaggerSchemaExample("MyLesson")]
        public string LessonPodType { get; set; } = string.Empty;
        public List<LessonPodSlideRequest> LessonUnitSlides { get; set; } = new List<LessonPodSlideRequest>();
    }
}
