using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.Password).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.DistrictName).NotNull().NotEmpty().Length(1, 100);
        }
    }
}
