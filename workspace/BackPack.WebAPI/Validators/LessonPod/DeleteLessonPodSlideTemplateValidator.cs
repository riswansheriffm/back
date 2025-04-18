using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class DeleteLessonPodSlideTemplateValidator : AbstractValidator<DeleteLessonPodSlideTemplateRequest>
    {
        public DeleteLessonPodSlideTemplateValidator()
        {
            RuleFor(request => request.SlideTemplateID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
