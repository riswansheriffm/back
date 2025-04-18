
using BackPack.Library.Repositories.Interfaces.ClassLink;
using BackPack.Library.Requests.ClassLink;
using BackPack.Library.Responses.ClassLink;
using BackPack.Library.Services.Interfaces.ClassLink;

namespace BackPack.Library.Services.Services.ClassLink
{
    public class ClassLinkService(
        IClassLinkRepository classLinkRepository
        ) : IClassLinkService
    {
        #region ClassLinkAsync
        public async Task<ClassLinkResponse> ClassLinkAsync(CreateClassLinkRequest request)
        {
            ClassLinkResponse response = await classLinkRepository.ClassLinkAsync(request);

            return response;
        }
        #endregion 
    }

}
