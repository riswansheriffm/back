using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class SuperUserValidator : AbstractValidator<SuperUserRequest>
    {
        public SuperUserValidator()
        {
            var userRole = new List<string?>() { "CurriculumAdmin" };

            RuleFor(request => request.DistrictID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.UserType).NotNull().NotEmpty().Must(request => userRole.Contains(request));
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.FName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.EmailID).NotNull().NotEmpty().EmailAddress();
        }
    }
}
