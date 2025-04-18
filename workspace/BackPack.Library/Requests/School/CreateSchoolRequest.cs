using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.School
{
    public class CreateSchoolRequest
    {
        [SwaggerSchema("School Name")]
        [SwaggerSchemaExample("SName")]
        public string? Name { get; set; }
        [SwaggerSchema("School Description")]
        [SwaggerSchemaExample("SDesc")]
        public string? Desc { get; set; }
        [SwaggerSchema("DistrictID")]
        [SwaggerSchemaExample("6440")]
        public int? DistrictID { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("12/02/2023")]
        public string? ActivityDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("10")]
        public int? ActivityBy { get; set; }
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
    }
}
