using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class DeleteCourseCapsuleValidator : AbstractValidator<DeleteCourseCapsuleRequest>
    {
        public DeleteCourseCapsuleValidator()
        {
            RuleFor(request => request.LoginID).NotEmpty();
            RuleFor(request => request.CourseCapsuleID).NotEmpty();
        }
    }
}
