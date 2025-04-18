using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class UpdateCourseCapsuleActivityValidator : AbstractValidator<UpdateCourseCapsuleActivityRequest>
    {
        public UpdateCourseCapsuleActivityValidator()
        {
            RuleFor(request => request.CourseCapsuleLessonPodID).NotNull();
            RuleFor(request => request.Activities).NotNull();
        }
    }
}
