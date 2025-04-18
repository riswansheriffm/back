using BackPack.Library.Requests.User;
using FluentValidation;

namespace BackPack.WebAPI.Validators.User
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentRequest>
    {
        public UpdateStudentValidator() 
        {
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.FName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LName).NotNull().NotEmpty().Length(1, 100);
        }
    }
}
