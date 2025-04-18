
using BackPack.Library.Responses.LessonPod.Distribution.SmartPaper;
using System.Text.Json.Serialization;

namespace BackPack.Library.Responses.LessonPod.Distribution.SmartCanvas
{
    public class SmartCanvasDropdownResponse
    {
        public int ControlID { get; set; }
        public int BoxPosition { get; set; }
        public int Score { get; set; }
        public string PluginName { get; set; } = string.Empty;
        public string LayoutType {  get; set; } = string.Empty;        
        public List<SmartCanvasDropdownOption> Options { get; set; } = [];
    }

    public class SmartCanvasDropdownOption : BaseAudioResponse
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<SmartPaperListDataResponse> Data { get; set; } = [];
        public int HighScore { get; set; }
        public int Priority { get; set; }
        public List<string> SpeechData { get; set; } = [];
        public List<string> Success { get; set; } = [];
        public List<string> Failure1 { get; set; } = [];
        public List<string> Failure2 { get; set; } = [];
        public List<string> Failure3 { get; set; } = [];
        public List<string> FeedbackOptions { get; set; } = [];
    }
}
