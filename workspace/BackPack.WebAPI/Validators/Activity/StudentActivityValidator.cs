using BackPack.Dependency.Library.Validation;
using BackPack.Library.Messages;
using BackPack.Library.Requests.Activity;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Activity
{
    public class StudentActivityValidator : AbstractValidator<StudentActivityRequest>
    {
        public StudentActivityValidator()
        {
            RuleFor(request => request.LessonUnitDistID).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(request => request.DomainID).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(request => request.ContentID).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(request => request.StudentID).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(request => request.Work).NotEmpty().NotNull();
            RuleFor(request => request.SubmitDate).NotEmpty().NotNull().Must(CommonValidation.CheckDateFormat).When(request => request.Offline == 1).WithMessage("'Submit Date' " + ActivityMessage.DateFormatError);
        }
    }
}
