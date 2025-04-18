using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class UpdateChapterValidator : AbstractValidator<UpdateChapterRequest>
    {
        public UpdateChapterValidator()
        {
            RuleFor(request => request.ChapterID).NotNull().NotEmpty();
        }
    }
}
