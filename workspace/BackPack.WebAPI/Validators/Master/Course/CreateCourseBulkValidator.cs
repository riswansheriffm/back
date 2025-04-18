using BackPack.Library.Requests.Master.Course;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Course
{
    public class CreateCourseBulkValidator : AbstractValidator<CreateCourseBulkRequest>
    {
        public CreateCourseBulkValidator()
        { 
            RuleFor(request => request.SchoolID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
