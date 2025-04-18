using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class CreateLessonPodSlideTemplateValidator : AbstractValidator<CreateLessonPodSlideTemplateRequest>
    {
        public CreateLessonPodSlideTemplateValidator()
        {
            var accessType = new List<string?>() { "Private", "Public" };

            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.TemplateName).NotNull().NotEmpty();
            RuleFor(request => request.AccessType).NotNull().NotEmpty().Must(request => accessType.Contains(request));
        }
    }
}
