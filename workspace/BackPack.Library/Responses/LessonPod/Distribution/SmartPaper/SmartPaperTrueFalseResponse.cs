namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperTrueFalseResponse : BaseControlResponse
    {
        public List<string>? Options { get; set; }
        public List<bool>? Answers { get; set; }
    }
}
