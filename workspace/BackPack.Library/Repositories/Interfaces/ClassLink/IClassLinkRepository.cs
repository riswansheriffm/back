
using BackPack.Library.Requests.ClassLink;
using BackPack.Library.Responses.ClassLink;

namespace BackPack.Library.Repositories.Interfaces.ClassLink
{
    public interface IClassLinkRepository
    {
        Task<ClassLinkResponse> ClassLinkAsync(CreateClassLinkRequest request);
    }

}
