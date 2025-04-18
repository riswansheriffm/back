using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class RecallLessonPodDistributionValidator : AbstractValidator<RecallLessonPodDistributionRequest>
    {
        public RecallLessonPodDistributionValidator()
        {
            RuleFor(request => request.LessonUnitDistID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
