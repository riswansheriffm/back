using BackPack.Tenant.Library.Requests;
using FluentValidation;

namespace BackPack.Tenant.WebAPI.Validators
{
    public class DeleteTenantValidator : AbstractValidator<DeleteTenantRequest>
    {
        public DeleteTenantValidator()
        {
            RuleFor(request => request.TenantID).NotNull().NotEmpty(); 
            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
        }

    }
}
