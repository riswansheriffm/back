using BackPack.Library.Requests.ClassLink;
using FluentValidation;

namespace BackPack.WebAPI.Validators.ClassLink
{
    public class ClassLinkValidator : AbstractValidator<CreateClassLinkRequest>
    {
        public ClassLinkValidator()
        {
            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
        }
    }

}
