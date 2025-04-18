using BackPack.Library.Requests.Master.Course;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Course
{
    public class MapTeacherToCourseValidator : AbstractValidator<BulkMapTeacherToCourseRequest>
    {
        public MapTeacherToCourseValidator()
        {
            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
        }
    } 
}
