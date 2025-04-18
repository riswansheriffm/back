namespace BackPack.Library.Responses.LessonPod.Distribution.SmartLabel
{
    public class SmartLabelResponse : BaseAudioResponse
    {
        public int TotalWeight { get; set; }
        public string? BackgroundImage { get; set; }
        public string? EditorImageWidth { get; set; }
        public string? EditorImageHeight { get; set; }
        public string? Opacity { get; set; }
        public List<SmartLabelBoxResponse> SmartLabelBoxes { get; set; } = [];
        public List<int> Seconds { get; set; } = [];
        public List<List<string>> TextToSpeech { get; set; } = [];
    }
}
