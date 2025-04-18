
using System.Text.Json.Serialization;

namespace BackPack.Library.Responses.LessonPod.Distribution
{
    public class BaseAudioResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? SpeechAudioData { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? OptionsAudioData { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? FeedbackAudioData { get; set; }
    }
}
