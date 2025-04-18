using BackPack.Library.Helpers.Common;
using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class LessonPodReDistributionValidator : AbstractValidator<LessonPodReDistributionRequest>
    {
        public LessonPodReDistributionValidator()
        {
            var whomToDistribute = new List<string?>() { "ALL", "SelectGroups", "SelectStudents" };

            RuleFor(request => request.LessonUnitDistID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.CourseID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.AuthorID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonName).NotNull().NotEmpty();
            RuleFor(request => request.LessonDesc).NotNull().NotEmpty();
            RuleFor(request => request.TargetDateOfCompletion).NotNull().NotEmpty().Must(Date.IsValidTargetDateFormat).Must(Date.IsTargetDateGreater);
            RuleFor(request => request.TargetTimeOfCompletion).NotNull().NotEmpty();
            RuleFor(request => request.WhomToDistribute).NotNull().NotEmpty().Must(request => whomToDistribute.Contains(request));
            RuleFor(request => request.WhomToDistribute).NotNull().NotEmpty();
            RuleFor(request => request.LessonUnitSlides.Count).GreaterThan(0);
            RuleForEach(request => request.LessonUnitSlides).SetValidator(new LessonPodSlideValidator());
        }
    }
}
