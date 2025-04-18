using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class CreatePreviewLessonPodActivityValidator : AbstractValidator<CreatePreviewLessonPodActivityRequest>
    {
        public CreatePreviewLessonPodActivityValidator()
        {
            RuleFor(request => request.LoginID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonJson).NotNull().NotEmpty();
        }
    }
}
