using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class CreateCourseCapsuleValidator : AbstractValidator<CreateCourseCapsuleRequest>
    {
        public CreateCourseCapsuleValidator()
        {
            RuleFor(request => request.DomainID).NotNull().NotEmpty();
            RuleFor(request => request.SubjectID).NotNull().NotEmpty();
            RuleFor(request => request.CourseCapsuleName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.CourseCapsuleID).NotNull();
        }
    }
}
