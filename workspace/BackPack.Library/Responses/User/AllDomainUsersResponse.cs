using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class AllDomainUsersResponse : ReadBaseResponse
    {
        public AllDomainUsersResponseData? Data { get; set; }
    }
    public class AllDomainUsersResponseData
    {
        public List<GetAllDomainUsersResult>? GetAllDomainUsersResults { get; set; }
    }
    public class GetAllDomainUsersResult
    {
        public int LoginID { get; set; }
        public string? FullName { get; set; }
        public string? LoginName { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? UserType { get; set; }
    }
}
