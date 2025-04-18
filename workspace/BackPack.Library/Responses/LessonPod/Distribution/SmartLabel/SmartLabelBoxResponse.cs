using System.Text.Json.Serialization;

namespace BackPack.Library.Responses.LessonPod.Distribution.SmartLabel
{
    public class SmartLabelBoxResponse : BaseAudioResponse
    {
        public int ControlID { get; set; }
        public int Rotation { get; set; }
        public int Weight { get; set; }
        public int FontSize { get; set; }
        public int Priority { get; set; }
        public bool CaseSensitive { get; set; }
        public string? XAxis { get; set; }
        public string? YAxis { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
        public string? SequenceHint { get; set; }
        public string? CorrectAnswerExplanation { get; set; }
        public string? IncorrectAnswerExplanation { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? BoxOverlay { get; set; }
    }
}
