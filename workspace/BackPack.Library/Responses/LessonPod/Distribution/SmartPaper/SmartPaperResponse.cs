namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperResponse
    {
        public int HighScore { get; set; }
        public float TotalScore { get; set; }
        public bool Back { get; set; }
        public bool Stop { get; set; }
        public bool Skip { get; set; }
        public bool ChatBot { get; set; }
        public bool IsCanvas { get; set; }
        public string? BackgroundImage { get; set; }
        public string? Title { get; set; }
        public string? EditorImageWidth { get; set; }
        public string? EditorImageHeight { get; set; }
        public string? Opacity { get; set; }
        public object? ChatBotSettings { get; set; }
        public object? TemplateSettings { get; set; }
        public object? SpeechAudioData { get; set; }
        public List<int>? Seconds { get; set; }
        public List<List<string>>? TextToSpeech { get; set; }
        public List<object>? InputRows { get; set; }
        public List<SmartPaperStrokeResponse>? Strokes { get; set; }
        public List<SmartPaperStrokeActionResponse>? StrokeActions { get; set; }
        public List<SmartPaperReplayRectangleResponse>? ReplayRectangles { get; set; }
    }
}
