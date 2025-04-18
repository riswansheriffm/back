using BackPack.Library.Requests.District;
using FluentValidation;

namespace BackPack.WebAPI.Validators.District
{
    public class UpdateDistrictValidator : AbstractValidator<UpdateDistrictRequest>
    {
        public UpdateDistrictValidator()
        {
            RuleFor(request => request.ID).NotNull().NotEmpty();
            RuleFor(request => request.Name).NotNull().NotEmpty();
            RuleFor(request => request.MaxStudents).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.FirstName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LastName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.EmailID).NotNull().NotEmpty().EmailAddress();
        }
    }
}
