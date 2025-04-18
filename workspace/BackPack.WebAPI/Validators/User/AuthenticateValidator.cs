using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class AuthenticateValidator : AbstractValidator<AuthenticateRequest>
    {
        public AuthenticateValidator() 
        {
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.Password).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.DomainName).NotNull().NotEmpty().Length(1, 100);
        }
    }
}
