
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.CourseCapsule
{
    public class PublishCourseCapsuleResponse : ReadBaseResponse
    {
        public PublishCourseCapsuleResponseData? Data { get; set; }
    }
    public class PublishCourseCapsuleResponseData
    {
        public GetPublicCourseCapsule? GetPublicCourseCapsule { get; set; }
    }
    public class GetPublicCourseCapsule
    {
        public List<SavePublishCourseCapsule>? SavePublishCourseCapsules { get; set; }
        public List<SavePublishCourseCapsuleLessonPod>? SavePublishCourseCapsuleLessonPods { get; set; }
        public List<CourseCapsuleSlide>? CourseCapsuleSlides { get; set; }
    }
}
