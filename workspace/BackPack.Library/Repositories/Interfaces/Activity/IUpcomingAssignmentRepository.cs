using BackPack.Library.Responses.Activity;

namespace BackPack.Library.Repositories.Interfaces.Activity
{
    public interface IUpcomingAssignmentRepository
    {
        Task<UpcomingAssignmentsResponse> UpcomingAssignmentsResponseAsync(int StudentID, string AssignmentDate);
    }
}
