using BackPack.Dependency.Library.Responses;
using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Repositories.Interfaces.Master.Document;
using BackPack.Library.Requests.Master.Document;
using BackPack.Library.Responses.Master.Document;
using BackPack.Library.Services.Interfaces.Master;

namespace BackPack.Library.Services.Services.Master
{
    public class DocumentService(
        ICreateDocumentRepository createDocumentRepository,
        IDocumentRepository documentRepository
        ) : IDocumentService 
    {
        #region CreateDocumentAsync
        public async Task<BaseResponse> CreateDocumentAsync(DocumentRequest request)
        {
            BaseResponse response = await createDocumentRepository.CreateDocumentAsync(request);

            return response;
        }
        #endregion 

        #region GetAllDocumentAsync
        public async Task<DocumentResponse> GetAllDocumentAsync(float[] EmbeddingVector, int Limit)
        {
            DocumentResponse response = await documentRepository.GetAllDocumentAsync(EmbeddingVector, Limit);

            return response;
        }
        #endregion
    }
}
