using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            var userRole = new List<string?>() { "DistrictAdmin", "SchoolAdmin", "CurriculumAdmin" };

            RuleFor(request => request.DistrictID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.UserType).NotNull().NotEmpty().Must(request => userRole.Contains(request));
            RuleFor(request => request.SchoolID).NotNull().NotEmpty().GreaterThan(0).When(request => request.UserType == "SchoolAdmin");
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.FName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LName).NotNull().NotEmpty().Length(1, 100);            
        }
    }
}
