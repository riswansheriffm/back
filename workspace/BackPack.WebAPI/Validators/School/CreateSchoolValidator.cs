using BackPack.Library.Requests.School;
using FluentValidation;

namespace BackPack.WebAPI.Validators.School
{
    public class CreateSchoolValidator : AbstractValidator<CreateSchoolRequest>
    {
        public CreateSchoolValidator()
        {
            RuleFor(request => request.Name).NotNull().NotEmpty();
            RuleFor(request => request.DistrictID).NotNull().NotEmpty();
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(2, 100);
            RuleFor(request => request.FirstName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LastName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.EmailID).NotNull().NotEmpty().EmailAddress();
        }
    }
}
