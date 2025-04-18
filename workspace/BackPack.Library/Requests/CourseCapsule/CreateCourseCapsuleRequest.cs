using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.CourseCapsule
{
    public class CreateCourseCapsuleRequest
    {
        [SwaggerSchema("CourseCapsuleName")]
        [SwaggerSchemaExample("Test123")]
        public string? CourseCapsuleName { get; set; }
        [SwaggerSchema("CourseCapsuleDesc")]
        [SwaggerSchemaExample("Test123")]
        public string? CourseCapsuleDesc { get; set; }
        [SwaggerSchema("ImageURL")]
        [SwaggerSchemaExample("https://nodeserver.learnpods.com/f4109835eafa5d8c5f744516563e33cf.jpg")]
        public string? ImageURL { get; set; }
        [SwaggerSchema("LoginID")]
        [SwaggerSchemaExample("546")]
        public int LoginID { get; set; }
        [SwaggerSchema("CourseCapsuleID")]
        [SwaggerSchemaExample("0")]
        public int CourseCapsuleID { get; set; }
        [SwaggerSchema("DomainID")]
        [SwaggerSchemaExample("104")]
        public int DomainID { get; set; }
        [SwaggerSchema("SubjectID")]
        [SwaggerSchemaExample("99")]
        public int SubjectID { get; set; }
        [SwaggerSchema("AppType")]
        [SwaggerSchemaExample("Knomadix")]
        public string? AppType { get; set; }
        public List<CreateCourseCapsuleLessonPod>? LessonPods { get; set; }
    }
    public class CreateCourseCapsuleLessonPod
    {
        [SwaggerSchema("LessonUnitID")]
        [SwaggerSchemaExample("4489")]
        public int LessonUnitID { get; set; }
        [SwaggerSchema("LessonID")]
        [SwaggerSchemaExample("2831")]
        public int LessonID { get; set; }
        [SwaggerSchema("CourseCapsuleFolderID")]
        [SwaggerSchemaExample("47")]
        public int CourseCapsuleFolderID { get; set; }
        [SwaggerSchema("LessonName")]
        [SwaggerSchemaExample("Our solar system")]
        public string? LessonName { get; set; }
        [SwaggerSchema("LessonDesc")]
        [SwaggerSchemaExample("Our solar system")]
        public string? LessonDesc { get; set; }
        [SwaggerSchema("AuthorID")]
        [SwaggerSchemaExample("2195")]
        public int AuthorID { get; set; }
        [SwaggerSchema("LessonPodVersion")]
        [SwaggerSchemaExample("22")]
        public int LessonPodVersion { get; set; }
        [SwaggerSchema("AccessType")]
        [SwaggerSchemaExample("Public")]
        public string? AccessType { get; set; }
        public List<CreateCourseCapsuleActivities>? Activities { get; set; }
    }
    public class CreateCourseCapsuleActivities
    {
        [SwaggerSchema("SlideID")]
        [SwaggerSchemaExample("pa-1648543273311")]
        public string? SlideID { get; set; }
        [SwaggerSchema("SlideType")]
        [SwaggerSchemaExample("Smart Label")]
        public string? SlideType { get; set; }
        [SwaggerSchema("SlideName")]
        [SwaggerSchemaExample("Order the Planets")]
        public string? SlideName { get; set; }
        [SwaggerSchema("IsSelected")]
        [SwaggerSchemaExample("1")]
        public int IsSelected { get; set; }
        [SwaggerSchema("ActivityType")]
        [SwaggerSchemaExample("practice")]
        public string? ActivityType { get; set; }
        [SwaggerSchema("MinScore")]
        [SwaggerSchemaExample("0")]
        public int MinScore { get; set; }
        [SwaggerSchema("MinTimeMin")]
        [SwaggerSchemaExample("0")]
        public int MinTimeMin { get; set; }
        [SwaggerSchema("MinTimeSec")]
        [SwaggerSchemaExample("0")]
        public int MinTimeSec { get; set; }
        [SwaggerSchema("MaxTimeMin")]
        [SwaggerSchemaExample("0")]
        public int MaxTimeMin { get; set; }
        [SwaggerSchema("MaxTimeSec")]
        [SwaggerSchemaExample("0")]
        public int MaxTimeSec { get; set; }
        [SwaggerSchema("FollowTheFlow")]
        [SwaggerSchemaExample("0")]
        public int FollowTheFlow { get; set; }
        [SwaggerSchema("AutoHint")]
        [SwaggerSchemaExample("0")]
        public int AutoHint { get; set; }
        [SwaggerSchema("ContentMode")]
        [SwaggerSchemaExample("false")]
        public string? ContentMode { get; set; }
        [SwaggerSchema("IsContainedView")]
        [SwaggerSchemaExample("true")]
        public bool IsContainedView { get; set; } = false;
    }
}
