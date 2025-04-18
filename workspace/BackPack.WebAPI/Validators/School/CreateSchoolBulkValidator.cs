using BackPack.Library.Requests.School;
using FluentValidation;

namespace BackPack.WebAPI.Validators.School
{
    public class CreateSchoolBulkValidator : AbstractValidator<CreateSchoolBulkRequest>
    {
        public CreateSchoolBulkValidator()
        {
            RuleFor(request => request.DistrictID).NotNull().NotEmpty();
            RuleFor(request => request.SchoolList).NotNull().NotEmpty();
        }
    }
}
