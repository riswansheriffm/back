using BackPack.Library.Requests.Master.Subject;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Subject
{
    public class CreateSubjectValidator : AbstractValidator<CreateSubjectRequest>
    {
        public CreateSubjectValidator()
        {
            RuleFor(request => request.DistrictID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
        }
    }
}
