
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.CourseCapsule
{
    public class AllLessonPodsBySubjectResponse : ReadBaseResponse
    {
        public AllLessonPodsBySubjectResult Data { get; set; } = new();
    }

    public class AllLessonPodsBySubjectResult
    {
        public List<AllLessonPodsBySubjectData> GetAllLessonPodsBySubjectResult { get; set; } = [];
    }

    public class AllLessonPodsBySubjectData : AllLessonBySubject
    {
        public List<AllLessonPodsBySubjectLessonpod> LessonPods { get; set; } = [];
    }

    public class AllLessonPodsBySubjectLessonpod
    {
        public int LessonUnitID { get; set; }
        public string LessonPodName { get; set; } = string.Empty;
        public string LessonPodDesc { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public int TotalActivities { get; set; }
        public int AuthorID { get; set; }
        public int LessonPodVersion { get; set; }

        public List<AllLessonPodsBySubjectActivity> PodActivities { get; set; } = [];
    }


    public class AllLessonPodsBySubjectActivity
    {
        public string SlideID { get; set; } = string.Empty;
        public string SlideName { get; set; } = string.Empty;
        public string SlideType { get; set; } = string.Empty;
        public bool IsContainedView { get; set; }
        public string ParentSlideID { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class AllLessonBySubject
    {
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class AllLessonByResultLessonpodQuery
    {
        public int LessonUnitID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public int TotalActivities { get; set; }
        public int AuthorID { get; set; }
        public int LessonPodVersion { get; set; }
    }
}
