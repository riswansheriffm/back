using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class UpdateLessonInLessonPodValidator : AbstractValidator<UpdateLessonInLessonPodRequest>
    {
        public UpdateLessonInLessonPodValidator()
        {
            RuleFor(request => request.LoginID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LessonUnitID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
