
namespace BackPack.Tenant.Library.Constants
{
    public static class ProcedureConstant
    {
        public const string GetTenants = "get_tenants";
        public const string CreateTenant = "create_tenant";
        public const string CreateServiceLog = "create_service_log";
        public const string UpdateServiceLog = "update_service_log";
        public const string DeleteTenant = "delete_tenant";
        public const string GetAllActiveTenants = "get_all_active_tenants";
        public const string GetAllPublicActivTenants = "get_all_public_active_tenants";
        public const string GetActiveTenant = "get_active_tenant";
        public const string GetTenantByTenantId = "get_tenant_by_tenant_id";
        public const string GetTenantByDomainId = "get_tenant_by_domain_id";
        public const string GetTenantByTenantName = "get_tenant_by_tenant_name";
        public const string GetUserForLogin = "get_user_for_login";
        public const string UpdateWrongPassword = "update_wrong_password";
        public const string ResetWrongPassword = "reset_wrong_password";
        public const string DeleteTenantByConsumerRejected = "delete_tenant_by_consumer_rejected";
        public const string UpdateTenantByConsumerAccepted = "update_tenant_by_consumer_accepted";
        public const string CreateRefreshToken = "create_refresh_token";
        public const string UpdateTenant = "update_tenant";
        public const string GetUserRefreshToken = "get_user_refresh_token";
    }
}
