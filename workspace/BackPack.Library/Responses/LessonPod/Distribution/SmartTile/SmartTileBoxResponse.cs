namespace BackPack.Library.Responses.LessonPod.Distribution.SmartTile
{
    public class SmartTileBoxResponse : BaseAudioResponse
    {
        public int ControlID { get; set; }
        public int Weight { get; set; }
        public string? TopSrc { get; set; }
        public string? BottomSrc { get; set; }
        public string? CorrectAnswerExplanation { get; set; }
        public string? IncorrectAnswerExplanation { get; set; }
        public string? SequenceHint { get; set; }
    }
}
