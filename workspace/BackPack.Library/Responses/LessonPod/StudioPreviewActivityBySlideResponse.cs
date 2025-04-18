
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class StudioPreviewActivityBySlideResponse : BaseResponse
    {
        public LPStudioPreviewActivityBySlideResult Data { get; set; } = new();
    }

    public class LPStudioPreviewActivityBySlideResult
    {
        public LPStudioPreviewActivityBySlideActivity GetLPStudioPreviewActivityBySlideResult { get; set; } = new();
    }

    public class LPStudioPreviewActivityBySlideActivity : LPStudioPreviewActivityBySlideData
    {
        public List<LPStudioPreviewActivityBySlideData> OtherActivities { get; set; } = [];
    }

    public class LPStudioPreviewActivityBySlideData
    {
        public string SlideID { get; set; } = string.Empty;
        public string SlideName { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string ActivityJson { get; set; } = string.Empty;
        public bool IsCanvas { get; set; } = false;
    }
}
