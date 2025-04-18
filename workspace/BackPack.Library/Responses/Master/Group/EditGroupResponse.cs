using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Group
{
    public class EditGroupResponse : ReadBaseResponse
    {
        public EditGroupData Data { get; set; } = new();
    }

    public class EditGroupData
    {
        public int GroupID { get; set; }
        public int CourseID { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string GroupDesc { get; set; } = string.Empty;
        public List<int> StudentsList { get; set; } = new();
    }

    public class StudentList
    {
        public int StudentID { get; set; }
    }
}
