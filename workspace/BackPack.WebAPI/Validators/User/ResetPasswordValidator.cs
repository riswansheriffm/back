using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.Password).NotNull().NotEmpty().MinimumLength(8)
                .MaximumLength(50).Matches(@"[A-Z]+").Matches(@"[a-z]+").Matches(@"[0-9]+").Matches(@"[\!\#\?\@\*\.]+");
            RuleFor(request => request.DistrictName).NotNull().NotEmpty().Length(1, 100);
        }
    }
}
