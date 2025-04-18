using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Grade
{
    public class AllGradeResponse : ReadBaseResponse
    {
        public List<AllGradeResponseData>? Data { get; set; }
    }
    public class AllGradeResponseData
    {
        public List<GetAllGradesResult>? GetAllGradesResult { get; set; }
    }
    public class GetAllGradesResult
    {
        public string? GradeDesc { get; set; }
        public string? GradeName { get; set; }
        public int GradeID { get; set; }
    }
}
