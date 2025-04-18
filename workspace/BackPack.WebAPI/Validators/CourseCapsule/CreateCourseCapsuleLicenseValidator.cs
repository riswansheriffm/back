using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class CreateCourseCapsuleLicenseValidator : AbstractValidator<CreateCourseCapsuleLicenseRequest>
    {
        public CreateCourseCapsuleLicenseValidator()
        {
            RuleFor(request => request.DomainID).NotNull().NotEmpty();
            RuleFor(request => request.LoginID).NotNull().NotEmpty();
            RuleFor(request => request.LicenseType).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.CourseCapsuleID).NotNull();
        }
    }
}

