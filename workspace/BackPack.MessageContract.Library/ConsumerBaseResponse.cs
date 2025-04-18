using System.Text.Json.Serialization;

namespace BackPack.MessageContract.Library
{
    public class ConsumerBaseResponse
    {
        public int MessageID { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; } = false;
        public string StatusMessage { get; set; } = string.Empty;
        [JsonIgnore]        
        public string? ExceptionType { get; set; }
        [JsonIgnore]
        public string? ExceptionMessage { get; set; }
    }
}
