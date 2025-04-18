using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Course;

namespace BackPack.Library.Responses.Master.Document
{
    public class DocumentResponse : ReadBaseResponse
    {
        public DocumentResponseData? Data { get; set; }
    }
    public class DocumentResponseData 
    { 
        public List<GetAllDocumentResult>? GetAllDocumentResult { get; set; }
    }
    public class GetAllDocumentResult
    { 
        public string? Content { get; set; } = string.Empty;
        public string? Source { get; set; } = string.Empty;
        public int Page { get; set; }
        public string? Title { get; set; } = string.Empty; 
    }
}
  