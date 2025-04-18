using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class StudentListForACourseResponse : ReadBaseResponse
    {
        public List<StudentListForACourseData> Data { get; set; } = new();
    }

    public class StudentListForACourseData
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; } = string.Empty;
    }
}
