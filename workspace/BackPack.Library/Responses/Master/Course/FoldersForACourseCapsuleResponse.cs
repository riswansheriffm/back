using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class FoldersForACourseCapsuleResponse : ReadBaseResponse
    {
        public FoldersForACourseCapsuleResponseData Data { get; set; } = new();
    }
    public class FoldersForACourseCapsuleResponseData
    {
        public List<GetAllCourseCapsuleFoldersForACourseCapsuleResult> GetAllCourseCapsuleFoldersForACourseCapsuleResult { get; set; } = [];
    }
    public class GetAllCourseCapsuleFoldersForACourseCapsuleResult
    {
        public int CourseCapsuleFolderID { get; set; } = 0;
        public int LoginID { get; set; } = 0;
        public int SubjectID { get; set; } = 0;
        public string? FolderName { get; set; } = string.Empty;
        public string? FolderDesc { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public int PodCount { get; set; } = 0;
    }
}
