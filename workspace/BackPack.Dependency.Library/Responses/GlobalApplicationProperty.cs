namespace BackPack.Dependency.Library.Responses
{
    public static class GlobalApplicationProperty
    {
        public static Guid TenantID { get; set; }
        public static string TenantName { get; set; } = string.Empty;
        public static string DBConnection { get; set; } = string.Empty;
        public static int UserID { get; set; }
        public static string UserType { get; set; } = string.Empty;
        public static string LoginName { get; set; } = string.Empty;
        public static int UserTypeID { get; set; }
        public static int DomainID { get; set; }
        public static string UserRole { get; set; } = string.Empty;
    }
}
