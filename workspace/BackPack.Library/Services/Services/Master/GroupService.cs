using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Requests.Master.Group;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Group;
using BackPack.Library.Services.Interfaces.Master;

namespace BackPack.Library.Services.Services.Master
{
    public class GroupService(IGroupRepository groupRepository) : IGroupService
    {
        #region CreateGroupAsync        
        public async Task<BaseResponse> CreateGroupAsync(CreateGroupRequest request)
        {
            BaseResponse response = await groupRepository.CreateGroupAsync(request);

            return response;
        }
        #endregion

        #region UpdateGroupAsync
        public async Task<BaseResponse> UpdateGroupAsync(UpdateGroupRequest request)
        {
            BaseResponse response = await groupRepository.UpdateGroupAsync(request);

            return response;
        }
        #endregion

        #region DeleteGroupAsync
        public async Task<BaseResponse> DeleteGroupAsync(DeleteGroupRequest request)
        {
            BaseResponse response = await groupRepository.DeleteGroupAsync(request);

            return response;
        }
        #endregion

        #region EditGroupAsync
        public async Task<EditGroupResponse> EditGroupAsync(int GroupID)
        {
            EditGroupResponse response = await groupRepository.EditGroupAsync(GroupID);

            return response;
        }
        #endregion

        #region ListGroupAsync
        public async Task<ListGroupResponse> ListGroupAsync(int CourseID)
        {
            ListGroupResponse response = await groupRepository.ListGroupAsync(CourseID);

            return response;
        }
        #endregion
    }
}
