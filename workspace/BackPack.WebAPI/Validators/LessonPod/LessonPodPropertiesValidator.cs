using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class LessonPodPropertiesValidator : AbstractValidator<LessonPodPropertiesRequest>
    {
        public LessonPodPropertiesValidator()
        {
            var accessType = new List<string?>() { "Private", "Public" };

            RuleFor(request => request.AuthorID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonUnitID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LessonDesc).MaximumLength(250);
            RuleFor(request => request.AccessType).NotNull().NotEmpty().Must(request => accessType.Contains(request));
        }
    }
}
