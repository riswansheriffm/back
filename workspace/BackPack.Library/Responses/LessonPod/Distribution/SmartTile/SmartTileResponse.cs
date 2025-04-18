namespace BackPack.Library.Responses.LessonPod.Distribution.SmartTile
{
    public class SmartTileResponse : BaseAudioResponse
    {
        public int TotalTiles { get; set; }
        public int TotalScore { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string? DocumentTitle { get; set; }
        public string? SlideName { get; set; }
        public string? TileRatio { get; set; }
        public string? TopTile { get; set; }
        public string? BottomTile { get; set; }
        public string? BackgroundImage { get; set; }
        public string? Opacity { get; set; }
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string? FontName { get; set; }
        public string? FontColor { get; set; }
        public string? FillColor { get; set; }
        public string? FontSize { get; set; }
        public List<List<SmartTileBoxResponse>>? SmartTileBoxes { get; set; }
        public List<List<SmartTileBoxResponse>>? DesktopSmartTileBoxes { get; set; }
        public List<int>? Seconds { get; set; }
        public List<List<string>>? TextToSpeech { get; set; }
    }
}
