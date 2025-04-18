using BackPack.Tenant.Library.Requests;
using FluentValidation;

namespace BackPack.Tenant.WebAPI.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidator()
        {
            var userType = new List<string?>() { "SuperAdmin" };

            RuleFor(request => request.UserID).NotNull().GreaterThan(0);
            RuleFor(request => request.RefreshToken).NotNull().NotEmpty();
            RuleFor(request => request.UserType).NotNull().NotEmpty().Must(request => userType.Contains(request));
        }
    }
}
