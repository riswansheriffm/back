using BackPack.Library.Requests.Master.Subject;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Subject
{
    public class DeleteSubjectValidator : AbstractValidator<DeleteSubjectRequest>
    {
        public DeleteSubjectValidator()
        {
            RuleFor(request => request.SubjectID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
        }
    }
}
