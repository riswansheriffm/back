using BackPack.Library.Requests.Grade;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Grade
{
    public class UpdateGradeValidator : AbstractValidator<UpdateGradeRequest>
    {
        public UpdateGradeValidator()
        {
            RuleFor(request => request.GradeID).NotNull().NotEmpty();
        }
    }
}
