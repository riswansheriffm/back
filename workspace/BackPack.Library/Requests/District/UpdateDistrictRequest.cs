using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.District
{
    public class UpdateDistrictRequest
    {
        [SwaggerSchema("District ID")]
        [SwaggerSchemaExample("7489")]
        public int ID { get; set; }
        [SwaggerSchema("District Name")]
        [SwaggerSchemaExample("DName")]
        public string? Name { get; set; }
        [SwaggerSchema("District Description")]
        [SwaggerSchemaExample("DDesc")]
        public string? Desc { get; set; }
        [SwaggerSchema("StreetAddress")]
        [SwaggerSchemaExample("StreetAddress")]
        public string? StreetAddress { get; set; }
        [SwaggerSchema("City")]
        [SwaggerSchemaExample("Frisco")]
        public string? City { get; set; }
        [SwaggerSchema("State")]
        [SwaggerSchemaExample("Texas")]
        public string? State { get; set; }
        [SwaggerSchema("ZipCode")]
        [SwaggerSchemaExample("ZC4905")]
        public string? ZipCode { get; set; }
        [SwaggerSchema("Maximum Student")]
        [SwaggerSchemaExample("10")]
        public int MaxStudents { get; set; }
        [SwaggerSchema("Maximum Teachers")]
        [SwaggerSchemaExample("10")]
        public int MaxTeachers { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("102")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("Email ID")]
        [SwaggerSchemaExample("tes@t.vm")]
        public string? EmailID { get; set; }
        [SwaggerSchema("First Name")]
        [SwaggerSchemaExample("FN")]
        public string? FirstName { get; set; }
        [SwaggerSchema("Last Name")]
        [SwaggerSchemaExample("LN")]
        public string? LastName { get; set; }
        [SwaggerSchema("Login Name")]
        [SwaggerSchemaExample("LogN")]
        public string? LoginName { get; set; }
        [SwaggerSchema("PhoneNo")]
        [SwaggerSchemaExample("457832931")]
        public string? PhoneNo { get; set; }

        [SwaggerSchema("Application ID")]
        [SwaggerSchemaExample("EC%2FAqRgB5yM%3D")]
        public string? ApplicationID { get; set; }
        [SwaggerSchema("Access Token")]
        [SwaggerSchemaExample("c41a41bd-7594-41e4-af30-e7429a25ebe5")]
        public string? AccessToken { get; set; }
        [SwaggerSchema("Source ID")]
        [SwaggerSchemaExample("LogN")]
        public string? SourceID { get; set; }
    }
}
