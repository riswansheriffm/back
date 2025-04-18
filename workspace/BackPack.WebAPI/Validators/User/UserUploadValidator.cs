using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class UserUploadValidator : AbstractValidator<UserUploadRequest>
    {
        public UserUploadValidator()
        {
            RuleFor(request => request.DistrictID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.SchoolID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty().GreaterThan(0);
            RuleForEach(request => request.UserList).SetValidator(new UserListUploadValidator());
        }
    }
}
