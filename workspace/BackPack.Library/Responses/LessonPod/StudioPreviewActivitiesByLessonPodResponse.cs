
using BackPack.Dependency.Library.Responses;
using System.Text.Json.Serialization;

namespace BackPack.Library.Responses.LessonPod
{
    public class StudioPreviewActivitiesByLessonPodResponse : BaseResponse
    {
        public StudioPreviewActivitiesByLessonPodData Data { get; set; } = new();
    }

    public class StudioPreviewActivitiesByLessonPodData
    {
        public StudioPreviewActivitiesByLessonPodResult GetLPStudioPreviewActivitiesByLessonPodResult { get; set; } = new();
    }

    public class StudioPreviewActivitiesByLessonPodResult
    {
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public List<StudioPreviewActivitiesByLessonPodActivityResult> Activities { get; set; } = [];
    }

    public class StudioPreviewActivitiesByLessonPodActivityResult
    {
        public string SlideID { get; set; } = string.Empty;
        public string SlideName { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        [JsonIgnore]
        public string ContainedViewData {  get; set; } = string.Empty;
        public List<object> ContainedView { get; set; } = [];
    }
}
