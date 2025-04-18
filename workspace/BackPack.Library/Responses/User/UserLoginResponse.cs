using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Responses.User
{
    public class UserLoginResponse : ReadBaseResponse
    {
        public TokenResponse? Token { get; set; }
        public UserLoginData? Data { get; set; }
    }

    public class UserLoginData
    {
        [SwaggerSchema("Domain ID")]
        [SwaggerSchemaExample("104")]
        public int DomainID { get; set; }

        [SwaggerSchema("Domain name")]
        [SwaggerSchemaExample("PISD")]
        public string? DomainName { get; set; }

        [SwaggerSchema("School ID")]
        [SwaggerSchemaExample("2")]
        public int SchoolID { get; set; }

        [SwaggerSchema("Firstname")]
        [SwaggerSchemaExample("William")]
        public string? FirstName { get; set; }

        [SwaggerSchema("Full name")]
        [SwaggerSchemaExample("William Michael")]
        public string? FullName { get; set; }

        [SwaggerSchema("Lastname")]
        [SwaggerSchemaExample("Michael")]
        public string? LastName { get; set; }

        [SwaggerSchema("Login ID")]
        [SwaggerSchemaExample("44334")]
        public int LoginID { get; set; }

        [SwaggerSchema("Login name")]
        [SwaggerSchemaExample("WilliamMichael")]
        public string? LoginName { get; set; }

        [SwaggerSchema("Role")]
        [SwaggerSchemaExample("Admin")]
        public string? UserRole { get; set; }
    }
}
