using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Grade
{
    public class GradeResponse : ReadBaseResponse
    {
        public GradeResponseData? Data { get; set; }
    }
    public class GradeResponseData
    {
        public GetGradeResult? GetGradeResult { get; set; }
    }
    public class GetGradeResult
    {
        public string? GradeDesc { get; set; } = string.Empty;
        public string? GradeName { get; set; } = string.Empty;
        public int GradeID { get; set; }
        public int DomainID { get; set; }
    }
}
