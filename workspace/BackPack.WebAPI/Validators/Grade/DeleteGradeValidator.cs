using BackPack.Library.Requests.Grade;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Grade
{
    public class DeleteGradeValidator : AbstractValidator<DeleteGradeRequest>
    {
        public DeleteGradeValidator()
        {
            RuleFor(request => request.GradeID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
        }
    }
}
