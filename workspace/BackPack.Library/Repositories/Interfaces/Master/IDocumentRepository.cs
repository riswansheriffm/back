using BackPack.Library.Responses.Master.Document;

namespace BackPack.Library.Repositories.Interfaces.Master
{
    public interface IDocumentRepository
    {
        Task<DocumentResponse> GetAllDocumentAsync(float[] EmbeddingVector, int Limit);
    }
}
  