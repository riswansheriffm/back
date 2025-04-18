using BackPack.Library.Requests.School;
using FluentValidation;

namespace BackPack.WebAPI.Validators.School
{
    public class UpdateSchoolValidator : AbstractValidator<UpdateSchoolRequest>
    {
        public UpdateSchoolValidator()
        {
            RuleFor(request => request.Name).NotNull().NotEmpty();
            RuleFor(request => request.ID).NotNull().NotEmpty();
        }
    }
}
