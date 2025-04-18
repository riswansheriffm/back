using BackPack.Library.Requests.Master.Group;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Group
{
    public class DeleteGroupValidator : AbstractValidator<DeleteGroupRequest>
    {
        public DeleteGroupValidator()
        {
            RuleFor(request => request.GroupID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
