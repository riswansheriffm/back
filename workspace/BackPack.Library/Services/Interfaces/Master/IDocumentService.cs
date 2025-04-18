using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.Master.Document;
using BackPack.Library.Responses.Master.Document;

namespace BackPack.Library.Services.Interfaces.Master
{
    public interface IDocumentService
    {
        Task<BaseResponse> CreateDocumentAsync(DocumentRequest request);

        Task<DocumentResponse> GetAllDocumentAsync(float[] EmbeddingVector, int Limit);
    }
}
