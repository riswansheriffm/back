using BackPack.Library.Requests.Master.Course;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Course
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseRequest>
    {
        public CreateCourseValidator()
        {
            RuleFor(request => request.DistrictID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
            RuleFor(request => request.CourseType).NotNull().NotEmpty().InclusiveBetween(0,2);
        }
    }
}
