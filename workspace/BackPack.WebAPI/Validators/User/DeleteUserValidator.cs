using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserValidator()
        {
            RuleFor(request => request.ID).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
