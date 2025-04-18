using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class UnlockOfflineLessonPodDistValidator : AbstractValidator<UnlockOfflineLessonPodDistRequest>
    {
        public UnlockOfflineLessonPodDistValidator()
        {
            RuleFor(request => request.LessonUnitDistID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
