using BackPack.Library.Requests.LessonPod;
using FluentValidation;

namespace BackPack.WebAPI.Validators.LessonPod
{
    public class DeleteChapterValidator : AbstractValidator<DeleteChapterRequest>
    {
        public DeleteChapterValidator()
        {
            RuleFor(request => request.ChapterID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
        }
    }
}
