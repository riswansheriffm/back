
namespace BackPack.MessageContract.Library
{
    public class CreateCourseCapsuleLicenseEvent
    {
        public int DomainID { get; set; }

        public int LoginID { get; set; }

        public int CourseCapsuleID { get; set; }

        public int CourseID { get; set; }

        public int ActivityBy { get; set; }

        public List<int>? StudentIDs { get; set; }

        public int Duration { get; set; }

        public string? WhomToLicense { get; set; }

        public DateTime StartDate { get; set; }

        public string? LicenseAction { get; set; }

        public string? LicenseType { get; set; }

        public string? DBConnection { get; set; }
    }
}
