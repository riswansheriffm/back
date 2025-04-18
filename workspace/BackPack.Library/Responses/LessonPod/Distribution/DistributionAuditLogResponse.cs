namespace BackPack.Library.Responses.LessonPod.Distribution
{
    public class DistributionAuditLogResponse
    {
        public int LessonUnitDistID { get; set; }
        public int UserID { get; set; }
        public int DistributionTypeID { get; set; }
        public string? ActivityDescription { get; set; }

    }
}
