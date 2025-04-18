using BackPack.Library.Requests.Token;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidator()
        {
            var userType = new List<string?>() { "Teacher", "Student" };

            RuleFor(request => request.UserID).NotNull().GreaterThan(0);
            RuleFor(request => request.RefreshToken).NotNull().NotEmpty();
            RuleFor(request => request.UserType).NotNull().NotEmpty().Must(request => userType.Contains(request));
        }
    }
}
