using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.Master.Document;

namespace BackPack.Library.Repositories.Interfaces.Master.Document
{
    public interface ICreateDocumentRepository
    {
        Task<BaseResponse> CreateDocumentAsync(DocumentRequest request);
    }
}
  