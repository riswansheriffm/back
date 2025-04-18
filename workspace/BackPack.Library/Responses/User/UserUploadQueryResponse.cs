namespace BackPack.Library.Responses.User
{
    public class UserUploadQueryResponse
    {
        public int ReturnStatus { get; set; }
        public int UserID { get; set; }
        public string DomainName { get; set; } = string.Empty;
    }
}
