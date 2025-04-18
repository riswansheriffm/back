namespace BackPack.Library.Responses.LessonPod.Distribution.SmartSlide
{
    public class SmartSlideInputControlResponse
    {
        public int ControlID { get; set; }
        public float TotalPoints { get; set; }
        public string? PluginName { get; set; }
        public string? ControlTag { get; set; }
        public string? ControlName { get; set; }
        public string? Question { get; set; }
        public List<SmartSlideInputControlDetailResponse>? InputControlDetails { get; set; }
    }
}
