﻿
namespace BackPack.MessageContract.Library
{
    public class BaseEvent
    {
        public int DomainId { get; set; }

        public int CourseID { get; set; }

        public int CourseCapsuleId { get; set; }

        public string DBConnection { get; set; } = string.Empty;
    }
}
