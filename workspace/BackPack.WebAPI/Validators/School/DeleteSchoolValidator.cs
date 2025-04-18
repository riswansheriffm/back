using BackPack.Library.Requests.School;
using FluentValidation;

namespace BackPack.WebAPI.Validators.School
{
    public class DeleteSchoolValidator : AbstractValidator<DeleteSchoolRequest>
    {
        public DeleteSchoolValidator()
        {
            RuleFor(request => request.ID).NotNull().NotEmpty();
        }
    }
}
