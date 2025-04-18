using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class CRLessonUnitDetailsResponse : ReadBaseResponse
    {
        public CRLessonUnitDetailsResult Data { get; set; } = new();
    }

    public class CRLessonUnitDetailsResult
    {
        public List<CRLessonUnitDetailsData> GetCRLessonUnitDetailsResult { get; set; } = [];
    }

    public class CRLessonUnitDetailsData
    {
        public int LessonUnitDistID { get; set; }
        public int ContentID { get; set; }
        public int TotalPoints { get; set; }
        public string ContentName { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
    }
}
