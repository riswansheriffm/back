using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.School
{
    public class CreateSchoolResponse : ReadBaseResponse
    {
        public int DomainID { get; set; }
        public int ReturnStatus { get; set; }
        public int UserID { get; set; }
        public string? DomainName { get; set; }
    }
} 
