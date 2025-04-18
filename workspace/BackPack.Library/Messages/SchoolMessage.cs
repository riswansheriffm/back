
namespace BackPack.Library.Messages
{
    public static class SchoolMessage
    {
        #region GetSchool
        public const string GetSchoolSummary = "Get School Details";
        public const string GetSchoolDescription = "Get School Details";
        #endregion

        #region GetAllSchool
        public const string GetAllSchoolSummary = "Get All School Details";
        public const string GetAllSchoolDescription = "Get All School Details";
        #endregion

        #region School
        public const string CreateSchoolSummary = "Create School";
        public const string CreateSchoolDescription = "Create School";

        public const string CreateSchoolBulkSummary = "School Bulk Upload";
        public const string CreateSchoolBulkDescription = "Create School Using excel bulk upload";

        public const string UpdateSchoolSummary = "Update School";
        public const string UpdateSchoolDescription = "Update School";
        public const string SchoolUpdate = "School Updated Successfully.";

        public const string DuplicateSchool = "School Already Exist.";
        public const string SchoolCreated = "School Created Successfully.";
        public const string UnknownError = "Something went wrong";
        public const string DuplicateUser = "User Already Exist.";

        public const string DeleteSchoolSummary = " Delete  School";
        public const string DeleteSchoolDescription = "Delete School";
        public const string SchoolDeleted = "School Deleted Successfully.";
        #endregion
    }
}
