namespace BackPack.Library.Requests.User
{
    public class UserUploadQueryRequest
    {
        public int DistrictID { get; set; }
        public int SchoolID { get; set; }
        public int ActivityBy { get; set; }
        public string LoginName { get; set; } = string.Empty;
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string EmailID { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string GmailID { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
    }
}
