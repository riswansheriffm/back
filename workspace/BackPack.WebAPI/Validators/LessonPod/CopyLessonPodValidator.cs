using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class CopyLessonPodValidator : AbstractValidator<CopyLessonPodRequest>
    {
        public CopyLessonPodValidator()
        {
            RuleFor(request => request.AuthorID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonUnitID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LessonDesc).MaximumLength(250);
        }
    }
}
