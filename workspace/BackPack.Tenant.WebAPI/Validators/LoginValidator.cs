using BackPack.Tenant.Library.Requests;
using FluentValidation;

namespace BackPack.Tenant.WebAPI.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            var userDomain = new List<string?>() { "knomadix" };

            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.Password).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.DistrictName.ToLower()).NotNull().NotEmpty().Length(1, 100).Must(request => userDomain.Contains(request));
        }
    }
}
