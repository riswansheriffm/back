using BackPack.Library.Responses.LessonPod.Distribution.SmartPaper;
using BackPack.Library.Responses.LessonPod.Distribution.SmartSlide;

namespace BackPack.Library.Responses.LessonPod.Distribution
{
    public class LessonPodSlideResponse
    {
        public int FileSize { get; set; }
        public float TotalPoints { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsCanvas { get; set; } = false;
        public string? AppName { get; set; }
        public string? SlideJson { get; set; }
        public string? SearchTag { get; set; }
        public string? SearchName { get; set; }
        public string? ImageURL { get; set; }
        public List<SmartPaperInputControlResponse>? SmartPaperInputControls { get; set; }
        public List<SmartPaperStrokeResponse>? SmartPaperStrokeReports { get; set; }
        public List<SmartPaperReplayRectangleReportResponse>? SmartPaperReplayRectangleReports { get; set; }
        public List<SmartSlideInputControlResponse>? SmartSlideInputControls { get; set; }

    }
}
