using BackPack.Library.Requests.Master.Group;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Group
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroupRequest>
    {
        public UpdateGroupValidator()
        {
            RuleFor(request => request.GroupID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.GroupName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.GroupDesc).NotNull().NotEmpty().MaximumLength(150);
            RuleFor(request => request.StudentsList!.Count).NotNull().GreaterThan(0);
        }
    }
}
