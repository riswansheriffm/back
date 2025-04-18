using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class UnlockOfflineLessonPodValidator : AbstractValidator<UnlockOfflineLessonPodRequest>
    {
        public UnlockOfflineLessonPodValidator()
        {
            RuleFor(request => request.LoginID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.CourseID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
