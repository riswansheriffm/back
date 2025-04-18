using BackPack.Library.Requests.District;
using FluentValidation;

namespace BackPack.WebAPI.Validators.District
{
    public class DistrictStatusValidator : AbstractValidator<DistrictStatusRequest>
    {
        public DistrictStatusValidator()
        {
            RuleFor(request => request.ID).NotNull().NotEmpty();
            RuleFor(request => request.ActivityBy).NotNull().NotEmpty();
        }
    }
}
