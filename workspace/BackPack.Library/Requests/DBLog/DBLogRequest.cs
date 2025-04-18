
namespace BackPack.Library.Requests.DBLog
{
    public class DBLogRequest
    {
        public Guid ServiceLogID { get; set; }
        public int LoginID { get; set; }
        public bool Success { get; set; }
        public string? LoginType { get; set; }
        public string? ServiceType { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceMethodName { get; set; }
        public string? ServiceRequest { get; set; }
        public string? ServiceResponse { get; set; }
        public string? ServiceStatus { get; set; }
        public string? LogMessage { get; set; }
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
    }
}
