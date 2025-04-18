namespace BackPack.Library.Requests.User
{
    public class UserUploadEmailRequest
    {
        public int SecurityCode { get; set; }
        public string? LoginName { get; set; }
        public string? EmailID { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? DomainName { get; set; }
        public string? HostName { get; set; }
    }
}
