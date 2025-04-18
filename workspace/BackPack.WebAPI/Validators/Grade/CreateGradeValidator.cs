using BackPack.Library.Requests.Grade;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Grade
{
    public class CreateGradeValidator : AbstractValidator<CreateGradeRequest>
    {
        public CreateGradeValidator()
        {
            RuleFor(request => request.DistrictID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
        }
    }
}
