using BackPack.Library.Requests.Master.Subject;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Subject
{
    public class UpdateSubjectValidator : AbstractValidator<UpdateSubjectRequest>
    {
        public UpdateSubjectValidator()
        {
            RuleFor(request => request.SubjectID).NotNull().NotEmpty();
        }
    }
}
