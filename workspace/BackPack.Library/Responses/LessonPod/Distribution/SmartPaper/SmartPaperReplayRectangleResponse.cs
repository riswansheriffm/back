using System.Text.Json.Serialization;

namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperReplayRectangleResponse
    {
        public int ControlID { get; set; }
        public int Delay { get; set; }
        public int Priority { get; set; }
        public int BoxPosition { get; set; }
        public bool Expand { get; set; }
        public string? XAxis { get; set; }
        public string? YAxis { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
        public string? ReplaySpeed { get; set; }
        public string? AudioUrl { get; set; }
        public string? StartInterval { get; set; }
        public List<string>? SpeechData { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? SpeechAudioData { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? OptionsAudioData { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? FeedbackAudioData { get; set; }
    }
}
