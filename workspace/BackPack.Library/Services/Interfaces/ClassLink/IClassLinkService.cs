
using BackPack.Library.Requests.ClassLink;
using BackPack.Library.Responses.ClassLink;

namespace BackPack.Library.Services.Interfaces.ClassLink
{
    public interface IClassLinkService
    {
        Task<ClassLinkResponse> ClassLinkAsync(CreateClassLinkRequest request);
    }

}
