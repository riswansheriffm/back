namespace BackPack.Library.Responses.LessonPod.Distribution.SmartSlide
{
    public class SmartSlideTrueFalseResponse : BaseControlResponse
    {
        public List<bool>? Answers { get; set; }
        public List<string>? Options { get; set; }
    }
}
