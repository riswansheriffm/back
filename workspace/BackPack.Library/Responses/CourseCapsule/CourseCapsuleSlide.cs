
namespace BackPack.Library.Responses.CourseCapsule
{
    public class CourseCapsuleSlide
    {
        public int CourseCapsuleActivityID { get; set; }
        public string? SlideID { get; set; }
        public string? SlideType { get; set; }
        public string? SlideName { get; set; }
        public string? ActivityType { get; set; }
        public float MinScore { get; set; }
        public int MinTimeMin { get; set; }
        public int MinTimeSec { get; set; }
        public int MaxTimeMin { get; set; }
        public int MaxTimeSec { get; set; }
        public bool FollowTheFlow { get; set; }
        public bool AutoHint { get; set; }
        public string? ContentMode { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsContainedView { get; set; }

    }
}
