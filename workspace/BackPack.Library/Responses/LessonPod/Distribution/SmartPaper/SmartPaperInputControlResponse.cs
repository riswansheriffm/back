namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperInputControlResponse
    {
        public int? ControlID { get; set; }
        public float? TotalPoints { get; set; }
        public float? Left { get; set; }
        public float? Top { get; set; }
        public string? PluginName { get; set; }
        public string? ControlTag { get; set; }
        public string? ControlName { get; set; }
        public List<SmartPaperInputControlDetailsResponse>? InputControlDetails { get; set; }
    }
}
