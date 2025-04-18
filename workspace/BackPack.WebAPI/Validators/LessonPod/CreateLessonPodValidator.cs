using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class CreateLessonPodValidator : AbstractValidator<CreateLessonPodRequest>
    {
        public CreateLessonPodValidator()
        {
            var accessType = new List<string?>() { "Private", "Public" };

            RuleFor(request => request.LessonID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.AuthorID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonName).NotNull().NotEmpty();
            RuleFor(request => request.LessonDesc).NotNull().NotEmpty();
            RuleFor(request => request.AccessType).NotNull().NotEmpty().Must(request => accessType.Contains(request));
            RuleFor(request => request.LessonJson).NotNull().NotEmpty();
        }
    }
}
