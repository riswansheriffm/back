using BackPack.Library.Requests.Master.Course;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Course
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseRequest>
    {
        public UpdateCourseValidator()
        {
            RuleFor(request => request.CourseID).NotNull().NotEmpty();
        }
    }
}
