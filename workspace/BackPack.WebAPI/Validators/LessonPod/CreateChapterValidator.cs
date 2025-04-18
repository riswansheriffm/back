using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class CreateChapterValidator : AbstractValidator<CreateChapterRequest>
    {
        public CreateChapterValidator() 
        {
            RuleFor(request => request.SubjectID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ChapterName).NotNull().NotEmpty();
        }
    }
}
