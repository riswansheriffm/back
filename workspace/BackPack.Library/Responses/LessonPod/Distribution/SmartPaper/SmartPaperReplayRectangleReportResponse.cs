namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperReplayRectangleReportResponse
    {
        public int ControlID { get; set; }
        public float XLeft { get; set; }
        public float XRight { get; set; }
        public float YTop { get; set; }
        public float YBottom { get; set; }
        public string? PluginName { get; set; }
        public string? ControlTag { get; set; }
        public string? ControlName { get; set; }
        public string? Height { get; set; }
        public string? Width { get; set; }
        public string? X { get; set; }
        public string? Y { get; set; }
    }
}
