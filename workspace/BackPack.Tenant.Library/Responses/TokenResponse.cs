using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Tenant.Library.Responses
{
    public class TokenResponse
    {
        [SwaggerSchema("Tenant ID")]
        [SwaggerSchemaExample("AF240982-739E-4392-B2E0-A3D9622F0E43")]
        public Guid TenantID { get; set; }

        [SwaggerSchema("User ID")]
        [SwaggerSchemaExample("44333")]
        public int UserID { get; set; }

        [SwaggerSchema("User Type")]
        [SwaggerSchemaExample("Student")]
        public string? UserType { get; set; }

        [SwaggerSchema("Access token")]
        [SwaggerSchemaExample("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyMTg0IiwibmJmIjoxNjUzOTc4OTA1LCJleHAiOjE2NTM5Nzk1MDUsImlhdCI6MTY1Mzk3ODkwNSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3QiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdCJ9.ZG2OrAG2Dtzd-FaWsbhlZ_mjPR8ltu1VhxzaFTCzJu8")]
        public string? AccessToken { get; set; }

        [SwaggerSchema("Refresh token")]
        [SwaggerSchemaExample("6U3fvZemBmFAwdkV7uPwos1DtkqKFKdq6KX7KYs2evo=")]
        public string? RefreshToken { get; set; }

        [SwaggerSchema("Token type")]
        [SwaggerSchemaExample("Bearer")]
        public string? TokenType { get; set; }

        [SwaggerSchema("Token expires in sec")]
        [SwaggerSchemaExample("600")]
        public int TokenExpiresIn { get; set; }

        [SwaggerSchema("Refresh token expires in sec")]
        [SwaggerSchemaExample("10080")]
        public int RefreshTokenExpiresIn { get; set; }
    }
}
