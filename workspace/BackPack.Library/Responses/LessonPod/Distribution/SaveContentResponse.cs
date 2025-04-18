namespace BackPack.Library.Responses.LessonPod.Distribution
{
    public class SaveContentResponse
    {
        public int ParentContentID { get; set; }
        public int LessonUnitDistID { get; set; }
        public int PublishedContentID { get; set; }
        public int LessonID { get; set; }
        public int LoginID { get; set; }
        public int CourseID { get; set; }
        public int DomainID { get; set; }
        public bool FlagVisibleToParent { get; set; }
        public bool IsContainedView { get; set; }
        public string? SlideID { get; set; }
        public string? ContentName { get; set; }
        public string? LessonPodType { get; set; }
    }
}
