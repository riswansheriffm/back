using BackPack.Library.Requests.Activity;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Activity
{
    public class TeacherFeedbackOnActivityValidator : AbstractValidator<TeacherFeedbackOnActivityRequest>
    {
        public TeacherFeedbackOnActivityValidator()
        {
            RuleFor(request => request.StudentID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ContentID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.AuthorID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.Rework).NotNull().NotEmpty().InclusiveBetween(0, 1);
            RuleFor(request => request.Feedback).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.Grade).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.Remarks).MaximumLength(1000);
        }
    }
}
