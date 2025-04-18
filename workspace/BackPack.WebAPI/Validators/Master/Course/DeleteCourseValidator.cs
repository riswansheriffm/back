using BackPack.Library.Requests.Master.Course;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Course
{
    public class DeleteCourseValidator : AbstractValidator<DeleteCourseRequest>
    {
        public DeleteCourseValidator()
        {
            RuleFor(request => request.CourseID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
        }
    }
}
