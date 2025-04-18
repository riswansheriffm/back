using BackPack.Library.Requests.Master.Group;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Group;

namespace BackPack.Library.Services.Interfaces.Master
{
    public interface IGroupService
    {
        Task<BaseResponse> CreateGroupAsync(CreateGroupRequest request);

        Task<BaseResponse> UpdateGroupAsync(UpdateGroupRequest request);

        Task<BaseResponse> DeleteGroupAsync(DeleteGroupRequest request);

        Task<EditGroupResponse> EditGroupAsync(int GroupID);

        Task<ListGroupResponse> ListGroupAsync(int CourseID);
    }
}
