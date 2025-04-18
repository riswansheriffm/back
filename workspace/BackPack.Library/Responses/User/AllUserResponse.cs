using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class AllUserResponse : ReadBaseResponse
    {
        public AllUsersResponseData? Data { get; set; }
    }
    public class AllUsersResponseData
    {
        public List<GetAllUsersResult>? GetAllUsersResults { get; set; }
    }
    public class GetAllUsersResult
    {
        public int LoginID { get; set; }
        public string? FullName { get; set; }
        public string? LoginName { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
    }
}
