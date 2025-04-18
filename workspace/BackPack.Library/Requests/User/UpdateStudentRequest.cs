using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class UpdateStudentRequest
    {
        [SwaggerSchema("LoginName")]
        [SwaggerSchemaExample("Test")]
        public string? LoginName { get; set; }
        [SwaggerSchema("FName")]
        [SwaggerSchemaExample("Test")]
        public string? FName { get; set; }
        [SwaggerSchema("LName")]
        [SwaggerSchemaExample("Test")]
        public string? LName { get; set;}
        [SwaggerSchema("EmailID")]
        [SwaggerSchemaExample("test@gmail.com")]
        public string? EmailID { get; set;}
        [SwaggerSchema("GmailID")]
        [SwaggerSchemaExample("test@gmail.com")]
        public string? GmailID { get; set;}
        [SwaggerSchema("DomainID")]
        [SwaggerSchemaExample("64")]
        public int DomainID { get; set;}
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("1")]
        public int ActivityBy { get; set;} 
    } 
}
 