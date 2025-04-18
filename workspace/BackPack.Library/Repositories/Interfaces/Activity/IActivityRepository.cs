using BackPack.Library.Responses.Activity;

namespace BackPack.Library.Repositories.Interfaces.Activity
{
    public interface IActivityRepository
    {
        Task<BackpackActivityForStudentResponse> BackpackActivityForStudentAsync(int StudentID, int ContentID, int ParentID);
    }
}
