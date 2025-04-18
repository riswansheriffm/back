namespace BackPack.MessageContract.Library
{
    public class CreateDomainEvent
    {
        public Guid TenantID { get; set; }
        public string? DBConnection { get; set; }
        public string? Country { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public int MaxStudents { get; set; }
        public int MaxTeachers { get; set; }
        public int ActivityBy { get; set; }
        public string? AccessType { get; set; }
        public string? EmailID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? LoginName { get; set; }
        public string? PhoneNo { get; set; }
        public string? ApplicationID { get; set; }
        public string? AccessToken { get; set; }
        public string? SourceID { get; set; }
    }
}
