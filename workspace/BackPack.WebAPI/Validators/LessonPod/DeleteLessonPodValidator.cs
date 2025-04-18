using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class DeleteLessonPodValidator : AbstractValidator<DeleteLessonPodRequest>
    {
        public DeleteLessonPodValidator()
        {
            RuleFor(request => request.LessonUnitID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.AuthorID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
