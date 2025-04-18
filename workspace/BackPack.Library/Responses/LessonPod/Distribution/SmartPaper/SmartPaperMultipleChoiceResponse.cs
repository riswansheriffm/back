
namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperMultipleChoiceResponse : BaseControlResponse
    {
        public List<int>? Answers { get; set; }

        public List<OptionControlResponse>? Options { get; set; }

        public object? ExtraPracticeComponents { get; set; }
    }
}
