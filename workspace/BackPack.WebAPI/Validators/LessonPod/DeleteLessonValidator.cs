using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class DeleteLessonValidator : AbstractValidator<DeleteLessonRequest>
    {
        public DeleteLessonValidator() 
        {
            RuleFor(request => request.LessonID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ActivityBy).NotNull();
        }
    }
}
