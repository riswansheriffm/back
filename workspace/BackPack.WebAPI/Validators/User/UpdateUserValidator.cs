using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginName).NotNull().NotEmpty();
        }
    }
}
