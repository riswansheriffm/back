namespace BackPack.Tenant.Library.Messages
{
    public static class TenantMessage
    {
        public const string CreateTenantSummary = "Create Tenant";
        public const string CreateTenantDescription = "Create Tenant and Tenant admin";

        public const string UpdateTenantSummary = "Update Tenant";
        public const string UpdateTenantDescription = "Update Tenant and Tenant admin";
        public const string UpdateTenantSuccess = "Tenant updated successfully.";

        public const string DeleteTenantSummary = "Delete Tenant";
        public const string DeleteTenantDescription = "Delete Tenant";
        public const string DeleteTenantSuccess = "Tenant deleted successfully.";

        public const string CreateTenantExist = "Tenant already exit.";
        public const string CreateTenantSuccess = "Tenant created successfully.";

        public const string GetTenantSummary = "Get Tenant details";
        public const string GetTenantDescription = "Get Tenant and user details";

        public const string GetAllTenantSummary = "Get all Tenant details";
        public const string GetAllTenantDescription = "Get all Tenant details";

        #region GetAllPublicActiveTenants
        public const string GetAllPublicActiveTenantsSummary = "Get All Public Active Tenants Details";
        public const string GetAllPublicActiveTenantsDescription = "Get All Public Active Tenants Details";
        #endregion

        #region GetAllActiveTenants
        public const string GetAllActiveTenantsSummary = "Get All Active Tenants Details";
        public const string GetAllActiveTenantsDescription = "Get All Active Tenants Details";
        #endregion
    }
}
