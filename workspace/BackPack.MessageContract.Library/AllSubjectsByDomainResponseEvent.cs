
using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{
    public class AllSubjectsByDomainResponseEvent : ReadBaseResponse
    {
        public AllSubjectsByDomainResponseEventData Data { get; set; } = new();
    }
    public class AllSubjectsByDomainResponseEventData
    {
        public List<AllSubjectsByDomainResponseEventResult> GetAllSubjectsByDomainResult { get; set; } = [];
    }
    public class AllSubjectsByDomainResponseEventResult
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }
}
