using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class DeleteStudentValidator : AbstractValidator<DeleteStudentRequest>
    {
        public DeleteStudentValidator()
        {
            RuleFor(request => request.ID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
