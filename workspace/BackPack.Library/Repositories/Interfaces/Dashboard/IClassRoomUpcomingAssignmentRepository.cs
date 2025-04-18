using BackPack.Library.Responses.Dashboard;

namespace BackPack.Library.Repositories.Interfaces.Dashboard
{
    public interface IClassRoomUpcomingAssignmentRepository
    {
        Task<ClassRoomUpcomingAssignmentsResponse> ClassRoomUpcomingAssignmentsResponseAsync(int AuthorID, string AssignmentDate);
    }
}
