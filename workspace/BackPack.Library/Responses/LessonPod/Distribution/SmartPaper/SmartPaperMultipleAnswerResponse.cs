namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperMultipleAnswerResponse : BaseControlResponse
    {
        public List<int>? Answers { get; set; }
        public List<OptionControlResponse>? Options { get; set; }
    }
}
