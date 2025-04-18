namespace BackPack.Library.Responses.Activity
{
    public class StrokeByContentResponse
    {
        public int ReplayRectangleID { get; set; }
        public int ContentID { get; set; }
        public int ControlID { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public string? X { get; set; }
        public string? Y { get; set; }
    }
}
