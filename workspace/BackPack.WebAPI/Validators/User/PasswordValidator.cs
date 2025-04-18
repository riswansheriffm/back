using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class PasswordValidator : AbstractValidator<PasswordRequest>
    {
        public PasswordValidator()
        {
            var userType = new List<string?>() { "Teacher", "Student" };

            RuleFor(request => request.FromUserID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ToUserID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.DistrictName).NotNull().NotEmpty();
            RuleFor(request => request.UserType).NotNull().NotEmpty().Must(request => userType.Contains(request));
        }
    }
}
