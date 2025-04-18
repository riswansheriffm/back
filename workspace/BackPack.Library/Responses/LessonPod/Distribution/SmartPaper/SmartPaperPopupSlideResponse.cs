namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperPopupSlideResponse : BaseAudioResponse
    {
        public int? ControlID { get; set; }
        public int? Priority { get; set; }
        public int? Delay { get; set; }
        public string? PluginId { get; set; }
        public string? X { get; set; }
        public string? Y { get; set; }
        public List<string>? SpeechData { get; set; }
        public SmartPaperPopupSlideMarkResponse? MarkData { get; set; }

    }
}
