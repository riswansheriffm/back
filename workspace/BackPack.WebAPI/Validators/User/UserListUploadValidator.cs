using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class UserListUploadValidator : AbstractValidator<UserListUploadRequest>
    {
        public UserListUploadValidator()
        {
            var userType = new List<string?>() { "Teacher", "Student" };

            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.FName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.UserType).NotNull().NotEmpty().Must(request => userType.Contains(request));
            RuleFor(request => request.EmailID).NotNull().NotEmpty().EmailAddress().When(request => request.UserType == "Teacher");
        }
    }
}
