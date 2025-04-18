namespace BackPack.Library.Repositories.Interfaces.Global
{
    public interface IGlobalRepository
    {
        Task<int> CheckDomainByID(int DomainID);

        Task<int> CheckDomainByName(string domainName);

        Task<int> CheckSchoolByID(int SchoolID);

        Task<int> CheckDomainAndSchoolByID(int DomainID, int SchoolID);

        Task<int> CheckCourseCapsuleByID(int DomainID, int SubjectID, int CourseCapsuleID, string CourseCapsuleName);

        Task<string> GetLoginNameByID(int userID, string userType);
    }
}
