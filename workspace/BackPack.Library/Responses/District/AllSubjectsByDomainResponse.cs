using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.District
{
    public class AllSubjectsByDomainResponse : ReadBaseResponse
    {
        public AllSubjectsByDomainResponseData? Data { get; set; }
    }
    public class AllSubjectsByDomainResponseData
    {
        public List<GetAllSubjectsByDomainResult>? GetAllSubjectsByDomainResult { get; set; }
    }
    public class GetAllSubjectsByDomainResult
    {
        public int SubjectID { get; set; }
        public string? SubjectName { get; set; } = string.Empty;
    }
}   
