using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.School
{
    public class CreateSchoolBulkRequest
    {
        [SwaggerSchema("DistrictID")]
        [SwaggerSchemaExample("6440")]
        public int? DistrictID { get; set; }
        [SwaggerSchema("ActivityDesc")]
        [SwaggerSchemaExample("12/02/2023")]
        public string? ActivityDesc { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("10")]
        public int? ActivityBy { get; set; }
        [SwaggerSchema("SchoolList")]
        [SwaggerSchemaExample("[{\"SchoolName\":\"sld\",\"SchoolDescrption\":\"sd\",\"FirstName\":\"sdf\",\"LastName\":\"sd\",\"LoginName\":\"sdf\",\"EmailID\":\"samchinnu001@gmail.com\",\"PhoneNo\":\"56054\"}]")]

        public List<SchoolList> SchoolList { get; set; } = new List<SchoolList>();
    }
    public class SchoolList
    {
        [SwaggerSchema("School Name")]
        [SwaggerSchemaExample("DName")]
        public string? SchoolName { get; set; }
        [SwaggerSchema("School Description")]
        [SwaggerSchemaExample("DDesc")]
        public string? SchoolDescrption { get; set; }
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
