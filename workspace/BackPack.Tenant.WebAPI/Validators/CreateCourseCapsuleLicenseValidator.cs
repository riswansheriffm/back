using BackPack.Tenant.Library.Requests;
using FluentValidation;

namespace BackPack.Tenant.WebAPI.Validators
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
