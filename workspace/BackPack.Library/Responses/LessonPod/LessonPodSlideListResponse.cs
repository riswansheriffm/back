namespace BackPack.Library.Responses.LessonPod
{
    public class LessonPodSlideListResponse
    {
        public int ContentID { get; set; }
        public int CourseCapsuleActivityID { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsContentView { get; set; }
        public bool IsCanvas { get; set; }
        public bool IsChildPage { get; set; }
        public string SlideID { get; set; } = string.Empty;
        public string SlideName { get; set; } = string.Empty;
        public string SlideType { get; set; } = string.Empty;
        public string DocumentTitle { get; set; } = string.Empty;
        public string ParentSlideID { get; set; } = string.Empty;
    }
}
