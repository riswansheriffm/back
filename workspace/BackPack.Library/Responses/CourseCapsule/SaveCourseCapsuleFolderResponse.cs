using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.CourseCapsule
{
    public class SaveCourseCapsuleFolderResponse : BaseResponse
    {
        public List<SaveCourseCapsuleFolderResponseData>? Data { get; set; }
    }
    public class SaveCourseCapsuleFolderResponseData
    {
        public List<SaveCourseCapsuleFolder>? SaveCourseCapsuleFolder { get; set; }
    }
    public class SaveCourseCapsuleFolder
    {
        public int CourseCapsuleFolderID { get; set; }
    }
}
