namespace BackPack.Library.Responses.LessonPod.Distribution.SmartPaper
{
    public class SmartPaperStrokeResponse
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public float Width { get; set; }
        public float ShapeWidth { get; set; }
        public float ShapeHeight { get; set; }
        public float ShapeTop { get; set; }
        public float ShapeLeft { get; set; }
        public float ShapeRadius { get; set; }
        public float ShapeRotation { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public bool IsDeleted { get; set; }
        public string? AuthorType { get; set; }
        public string? Color { get; set; }
        public string? ShapeType { get; set; }
        public List<List<float>>? Data { get; set; }

    }
}
