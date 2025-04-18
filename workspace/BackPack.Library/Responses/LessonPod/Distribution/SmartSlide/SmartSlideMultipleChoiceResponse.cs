namespace BackPack.Library.Responses.LessonPod.Distribution.SmartSlide
{
    public class SmartSlideMultipleChoiceResponse : BaseControlResponse
    {
        public List<float>? Answers { get; set; }
        public List<OptionControlResponse>? Options { get; set; }
    }
}
