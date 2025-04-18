using BackPack.Library.Helpers.Common;
using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class LessonPodSlideValidator : AbstractValidator<LessonPodSlideRequest>
    {
        public LessonPodSlideValidator()
        {
            var activityType = new List<string?>() { "practice", "test" };

            RuleFor(request => request.SlideID).NotNull().NotEmpty().Length(1, 250);
            RuleFor(request => request.TargetDateOfCompletion).NotNull().NotEmpty().Must(Date.IsValidTargetDateFormat).Must(Date.IsTargetDateGreater);
            RuleFor(request => request.TargetTimeOfCompletion).NotNull().NotEmpty();
            RuleFor(request => request.ActivityType).NotNull().NotEmpty().Must(request => activityType.Contains(request));
        }
    }
}
