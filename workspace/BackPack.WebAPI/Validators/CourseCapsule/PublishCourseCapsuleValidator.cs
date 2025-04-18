using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class PublishCourseCapsuleValidator : AbstractValidator<PublishCourseCapsuleRequest>
    {
        public PublishCourseCapsuleValidator()
        {
            RuleFor(request => request.LoginID).NotNull().NotEmpty();
            RuleFor(request => request.CourseCapsuleID).NotNull();
        }
    }
}
