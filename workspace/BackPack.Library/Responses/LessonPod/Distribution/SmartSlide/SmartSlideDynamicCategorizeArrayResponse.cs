namespace BackPack.Library.Responses.LessonPod.Distribution.SmartSlide
{
    public class SmartSlideDynamicCategorizeArrayResponse
    {
        public int? ColumnCount { get; set; }
        public float? Score { get; set; }
        public string? ID { get; set; }
        public string? AnswerLabel { get; set; }
        public string? Color { get; set; }
        public string? X { get; set; }
        public string? Y { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
        public List<SmartSlideDynamicCategorizeAnswerResponse>? Answers { get; set; }
    }
}
